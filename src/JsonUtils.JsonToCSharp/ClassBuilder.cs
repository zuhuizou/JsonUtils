using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace JsonUtils.JsonToCSharp
{
    public class ClassBuilder
    {
        /// <summary>
        /// 获得类集合
        /// </summary>
        /// <param name="childrenTokens"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<StringBuilder> GetClassBuilder(string json, string rootClassName)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(json);
            if (obj == null) return null;

            return GetClassBuilder(obj, rootClassName);
        }

        /// <summary>
        /// 获得类集合
        /// </summary>
        /// <param name="childrenTokens"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private List<StringBuilder> GetClassBuilder(IEnumerable<JToken> childrenTokens, string className)
        {
            if (childrenTokens == null) return new List<StringBuilder>();

            List<StringBuilder> sbs = new List<StringBuilder>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public class {className ?? Guid.NewGuid().ToString()}");
            sb.AppendLine("{");

            sbs.Add(sb);

            foreach (var item in childrenTokens)
            {
                string path = FileNameHandle(item.Path);
                sb.AppendLine($"    public {GetFieldType(item, path + "Item", sbs)} " + path + " { get; set; }");
            }

            sb.AppendLine("}");

            return sbs;
        }

        /// <summary>
        /// 字段名称处理
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FileNameHandle(string path)
        {
            int dotIndex = path.LastIndexOf('.');

            if (dotIndex > -1)
            {
                return path.Substring(dotIndex + 1);
            }
            else
                return path;
        }

        /// <summary>
        /// 获得字段类型
        /// </summary>
        /// <param name="field"></param>
        /// <param name="childClassName"></param>
        /// <param name="bulder"></param>
        /// <returns></returns>
        public string GetFieldType(JToken field, string childClassName, List<StringBuilder> bulder)
        {
            JTokenType type = JTokenType.None;

            try
            {
                type = field.Children().First().Type;
            }
            catch
            {
                type = field.Type;
            }

            switch (type)
            {
                case JTokenType.Object:
                    bulder.AddRange(GetClassBuilder(field.Children().FirstOrDefault(), childClassName));//子类处理
                    return childClassName;
                case JTokenType.Array:
                    var element = field.Children().First().FirstOrDefault();
                    if (element != null)
                    {
                        if (element.Type == JTokenType.String)
                            return "List<string>";
                        else if (element.Type == JTokenType.Integer)
                            return "List<long>";
                        else if (element.Type == JTokenType.Float)
                            return "List<float>";
                        else if (element.Type == JTokenType.Date)
                            return "List<DateTime>";
                        else if (element.Type == JTokenType.Boolean)
                            return "List<bool>";
                    }

                    bulder.AddRange(GetClassBuilder(element, childClassName));//数组处理
                    return $"List<{childClassName}>";
                case JTokenType.String:
                    return "string";
                case JTokenType.Date:
                    return "DateTime";
                case JTokenType.Integer:
                    return "long";
                case JTokenType.Boolean:
                    return "bool";
                case JTokenType.Float:
                    return "float";
                case JTokenType.Bytes:
                    return "byte[]";
                case JTokenType.Null:
                    throw new ArgumentException("请确保每个字段都有值，否则无法确认字段类型");
                default:
                    return "string";
            }
        }
    }
}
