/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2014 Michael Chatfield - http://xmlcomment.codeplex.com/
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, 
 * including without limitation the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlCommentSerialization
{
    public sealed class XmlCommentElement : IXmlSerializable
    {

        // TODO: Handle Collections (Arrays etc)
        // TODO: Separate attributes from elements Test with Elements
        // TODO: Determine .NET Framework Compile Version.
        // TODO: Cache supplemental descriptions for enum constants
        // TODO: XmlElementAttribute IsNullable?
        // TODO: Understand XmlSchema and how it applies.
        // TODO: Change Prefix to boolean flag that indicates if the default annotation should still be included when comment specified.
        // TODO: Determine what types can be removed from the TypeCache because they're not supported in Xml.

        // Identifies and allow suppression of Comment element by XmlCommentWriter
        public const string Namespace = "http://www.30e6b941-728b-4a0d-b215-56fe5c470ff9.org/";
        public const string ElementName = "Comment";

        private readonly Type _Type;
        
        // Instance variables to allow for variation with constructor
        private int _MaxLength;
        private bool? _Metadata;
        private bool? _Repeat;
        private bool? _Alphabetize;

        private static Dictionary<Type, string> _TypeCache;
        private static Dictionary<Type, string> _TypeDescriptionCache;
        private static Dictionary<Type, List<Metadata>> _MemberCache;
        private static BindingFlags _BindingFlags;
        private static Type _DescriptionAttributeType;
        private static Type _XmlAttributeAttributeType;
        private static Type _XmlElementAttributeType;
        private static Type _FlagsAttributeType;
        private static Type _XmlCommentAttributeType;
        private static Type _XmlArrayItemAttribute;
        private static Type _DefaultValueAttribute;
        private static Type _XmlEnumAttribute;
        private static Type _ThisType;

        private struct Metadata
        {
            public string Name; // Given name for the type
            public XmlElementAttribute XmlElement;
            public XmlAttributeAttribute XmlAttribute;
            public DescriptionAttribute Description;
            public XmlCommentAttribute Comment;
            public DefaultValueAttribute Default;
            public object[] ArrayItems;
            public Type Type;
        }

        #region Public constructors
        public XmlCommentElement() { } // Required for deserialization
        public XmlCommentElement(Type type) { _Type = type; }
        public XmlCommentElement(Type type, int maxCommentLength,
            bool metadataComments, bool repeatComments, 
            bool alphabetizeComments)
        {
            _Type = type;
            _MaxLength = maxCommentLength;
            _Metadata = metadataComments;
            _Repeat = repeatComments;
            _Alphabetize = alphabetizeComments;
        }
        #endregion

        #region Public methods

        public XmlSchema GetSchema()
        {
            return null; // No schema required.
        }

        public void ReadXml(XmlReader reader)
        {
            return; // Comments don't deserialize
        }

        public void WriteXml(XmlWriter writer)
        {

            if (writer == null) throw new ArgumentNullException(Resource.ExceptionWriterNull);
            if (_Type == null) throw new NotSupportedException(Resource.ExceptionInvalidState);

            #region Alter XmlWriter output behaviour, capture maximum comment length
            if (writer is XmlCommentWriter && !_Metadata.HasValue)
            {
                XmlCommentWriter annotatedWriter = (XmlCommentWriter)writer;
                if (annotatedWriter.MaxLength > 0) // Ignore negative values
                    _MaxLength = annotatedWriter.MaxLength;
                _Metadata = annotatedWriter.Metadata;
                _Repeat = annotatedWriter.Repeat;
                _Alphabetize = annotatedWriter.Alphabetize;
            }
            #endregion

            #region Cache *Static* Type Descriptions and Attributes
            if (_TypeCache == null)
            {
                _TypeCache = new Dictionary<Type, string>();
                _TypeDescriptionCache = new Dictionary<Type, string>();
                _TypeCache.Add(typeof(Boolean), Resource.TypeBoolean);
                _TypeCache.Add(typeof(Byte), Resource.TypeByte);
                _TypeDescriptionCache.Add(typeof(Byte), String.Format(Resource.TypeByte, 
                    Byte.MinValue, Byte.MaxValue));
                _TypeCache.Add(typeof(SByte), Resource.TypeByte);
                _TypeDescriptionCache.Add(typeof(SByte), String.Format(Resource.TypeByte,
                    SByte.MinValue, SByte.MaxValue));
                _TypeCache.Add(typeof(Int16), Resource.TypeInt);
                _TypeDescriptionCache.Add(typeof(Int16), String.Format(Resource.TypeInt,
                    Int16.MinValue, Int16.MaxValue));
                _TypeCache.Add(typeof(UInt16), Resource.TypeUInt);
                _TypeDescriptionCache.Add(typeof(UInt16), String.Format(Resource.TypeInt,
                    UInt16.MinValue, UInt16.MaxValue));
                _TypeCache.Add(typeof(Int32), Resource.TypeInt);
                _TypeDescriptionCache.Add(typeof(Int32), String.Format(Resource.TypeInt,
                    Int32.MinValue, Int32.MaxValue));
                _TypeCache.Add(typeof(UInt32), Resource.TypeUInt);
                _TypeDescriptionCache.Add(typeof(UInt32), String.Format(Resource.TypeInt,
                    UInt32.MinValue, UInt32.MaxValue));
                _TypeCache.Add(typeof(Int64), Resource.TypeInt);
                _TypeDescriptionCache.Add(typeof(Int64), String.Format(Resource.TypeInt,
                    Int64.MinValue, Int64.MaxValue));
                _TypeCache.Add(typeof(UInt64), Resource.TypeUInt);
                _TypeDescriptionCache.Add(typeof(UInt64), String.Format(Resource.TypeUInt,
                    UInt64.MinValue, UInt64.MaxValue));
                _TypeCache.Add(typeof(IntPtr), Resource.TypeIntPtr);
                _TypeDescriptionCache.Add(typeof(IntPtr), Resource.TypeIntPtr);
                _TypeCache.Add(typeof(UIntPtr), Resource.TypeUIntPtr);
                _TypeDescriptionCache.Add(typeof(UIntPtr), Resource.TypeUIntPtr);
                _TypeCache.Add(typeof(Char), Resource.TypeChar);
                _TypeDescriptionCache.Add(typeof(Char), String.Format(Resource.TypeChar,
                    Char.MinValue, Char.MaxValue));
                _TypeCache.Add(typeof(Double), Resource.TypeDouble);
                _TypeDescriptionCache.Add(typeof(Double), String.Format(Resource.TypeDouble,
                    Double.MinValue, Double.MaxValue));
                _TypeCache.Add(typeof(Single), Resource.TypeSingle);
                _TypeDescriptionCache.Add(typeof(Single), String.Format(Resource.TypeSingle,
                    Single.MinValue, Single.MaxValue));
                _TypeCache.Add(typeof(String), Resource.TypeString);
                _TypeDescriptionCache.Add(typeof(String), String.Format(Resource.TypeString,
                    0, Int32.MaxValue));
                _MemberCache = new Dictionary<Type, List<Metadata>>();
                _BindingFlags = BindingFlags.Public | BindingFlags.Instance;
                _DescriptionAttributeType = typeof(DescriptionAttribute);
                _XmlAttributeAttributeType = typeof(XmlAttributeAttribute);
                _XmlElementAttributeType = typeof(XmlElementAttribute);
                _FlagsAttributeType = typeof(FlagsAttribute);
                _XmlCommentAttributeType = typeof(XmlCommentAttribute);
                _XmlArrayItemAttribute = typeof(XmlArrayItemAttribute);
                _DefaultValueAttribute = typeof(DefaultValueAttribute);
                _XmlEnumAttribute = typeof(XmlEnumAttribute);
                _ThisType = typeof(XmlCommentElement);
            }
            #endregion

            #region Check if repeating output of Type comments allowed
            if (_TypeCache.ContainsKey(_Type) && (!_Repeat.HasValue || 
                _Repeat.HasValue && !_Repeat.Value)) return;
            #endregion

            #region Cache Member Metadata (if required)
            List<Metadata> metadata;
            if (_MemberCache.ContainsKey(_Type))
                metadata = _MemberCache[_Type];
            else
            {

                // Get all Fields and Properties of Type implementing Comment class
                MemberInfo[] members = _Type.GetFields(_BindingFlags).Cast<MemberInfo>().Concat(_Type.GetProperties(_BindingFlags)).ToArray();
                metadata = new List<Metadata>();

                // Cache metadata array even if it's zero length.
                _MemberCache.Add(_Type, metadata);

                // Populate metadata
                for (int i = 0; i < members.Length; i++)
                {

                    // Supports Field or Property members
                    FieldInfo field = null;
                    PropertyInfo property = null;
                    MemberInfo member = members[i];
                    if (member is FieldInfo) field = (FieldInfo)member;
                    else property = (PropertyInfo)member;

                    // Capture Attribute and Type data (may skip this member)
                    Metadata data = new Metadata();
                    data.Type = (field != null) ? field.FieldType : property.PropertyType;
                    if (data.Type == _ThisType) continue; // Ignore this class
                    data.Comment = (XmlCommentAttribute)member.GetCustomAttribute(_XmlCommentAttributeType);
                    if (data.Comment != null && data.Comment.Ignore) continue; // Explicit Ignore
                    if (data.Comment == null && _Metadata.HasValue && !_Metadata.Value) continue; // Not explicit and no XmlComment 
                    data.XmlElement = (XmlElementAttribute)member.GetCustomAttribute(_XmlElementAttributeType);
                    data.XmlAttribute = (XmlAttributeAttribute)member.GetCustomAttribute(_XmlAttributeAttributeType);
                    data.Description = (DescriptionAttribute)member.GetCustomAttribute(_DescriptionAttributeType);
                    data.ArrayItems = (XmlArrayItemAttribute[])member.GetCustomAttributes(_XmlArrayItemAttribute);
                    data.Default = (DefaultValueAttribute)member.GetCustomAttribute(_DefaultValueAttribute);

                    // Determine name based on Attribute, Element or Type
                    data.Name = (data.XmlElement != null) ? data.XmlElement.ElementName :
                        (data.XmlAttribute != null) ? data.XmlAttribute.AttributeName :
                        member.Name;

                    // Cache data
                    metadata.Add(data);

                }

                // Alphabetize if required
                if (_Alphabetize.HasValue && _Alphabetize.Value)
                {
                    SortedList<string, Metadata> sortedMetadata = new SortedList<string,Metadata>();
                    foreach (Metadata data in metadata) sortedMetadata.Add(data.Name, data);
                    metadata.Clear();
                    foreach (string key in sortedMetadata.Keys) metadata.Add(sortedMetadata[key]);
                }

            }
            #endregion

            #region Output type Description (if required)
            if (!_TypeCache.ContainsKey(_Type))
            {
                DescriptionAttribute description = (DescriptionAttribute)_Type.GetCustomAttribute(_DescriptionAttributeType);
                _TypeCache.Add(_Type, (description == null) ? null : description.Description);
            }
            if (_TypeCache[_Type] != null)
                this.WriteXmlComment(writer, _TypeCache[_Type]);
            #endregion

            #region Output reflected member comments to XmlWriter
            foreach (Metadata data in metadata)
            {

                string[] supplementalComments = null;

                string comment;
                if (data.Comment != null && data.Comment.Comment != null)
                    comment = data.Comment.Comment;
                else if (data.Description != null)
                    comment = data.Description.Description;
                else if (data.Type.IsEnum && !_TypeCache.ContainsKey(data.Type))
                {
                    // Cache Enum Types to be optimal if referenced multiple times
                    bool flags = data.Type.GetCustomAttribute(_FlagsAttributeType) != null;
                    string[] constantNames = Enum.GetNames(data.Type);
                    for (int i = 0; i < constantNames.Length; i++)
                    {
                        MemberInfo member = (MemberInfo)data.Type.GetMember(constantNames[i])[0];
                        XmlEnumAttribute xmlConstant = (XmlEnumAttribute)member.GetCustomAttribute(_XmlEnumAttribute);
                        DescriptionAttribute description = (DescriptionAttribute)member.GetCustomAttribute(_DescriptionAttributeType);
                        if (xmlConstant != null && xmlConstant.Name != null)
                            constantNames[i] = xmlConstant.Name;
                        if (description != null && description.Description != null)
                        {
                            if (supplementalComments == null) 
                                supplementalComments = new string[constantNames.Length];
                            supplementalComments[i] = String.Format(Resource.Supplemental,
                                Resource.CommentWrap, constantNames[i], description.Description);
                        }
                    }
                    if (supplementalComments != null)
                        _TypeCache.Add(data.Type, String.Format(flags ? Resource.TypeEnumFlags :
                            Resource.TypeEnum, String.Format(Resource.TypeEnumSuffix,
                            Resource.CommentWrap)));
                    else
                        _TypeCache.Add(data.Type, String.Format(flags ? Resource.TypeEnumFlags :
                            Resource.TypeEnum, String.Join(", ", constantNames)));
                    comment = _TypeCache[data.Type];
                }
                else if (_TypeCache.ContainsKey(data.Type))
                {
                    if (data.Comment != null && data.Comment.Min != null && data.Comment.Max != null)
                        comment = String.Format(_TypeCache[data.Type], 
                            data.Comment.Min, data.Comment.Max);
                    else if (_TypeDescriptionCache.ContainsKey(data.Type))
                        comment = _TypeDescriptionCache[data.Type];
                    else
                        comment = _TypeCache[data.Type];
                }
                else if (data.ArrayItems.Length == 1)
                {
                    // TODO: If element name is null, get the type name from the ICollection
                    comment = String.Format(Resource.TypeArrayOne,
                        ((XmlArrayItemAttribute)data.ArrayItems[0]).ElementName);
                }
                else if (data.ArrayItems.Length > 0)
                {
                    // TODO: If element name is null, get the type name from the ICollection
                    string elementNames = null;
                    foreach (XmlArrayItemAttribute arrayItem in data.ArrayItems)
                    {
                        if (elementNames != null) elementNames += ", " + arrayItem.ElementName;
                        else elementNames = arrayItem.ElementName;
                    }
                    comment = String.Format(Resource.TypeArrayMany, elementNames);
                }
                else
                    comment = data.Name;

                // Include additional prefix if specified.
                if (data.Comment != null && data.Comment.Prefix != null)
                {
                    string description = data.Comment.Prefix.Trim();
                    if (description[description.Length - 1] != '.') description += ".";
                    comment = String.Format("{0} {1}", description, comment);
                }

                // Include default value if specified.
                string suffix = null;
                if (data.Comment != null && data.Comment.Default != null)
                {
                    if (data.Comment.Default is bool) // Handle incorrect case boolean
                        suffix = String.Format(Resource.Default, 
                            data.Comment.Default.ToString().ToLower());
                    else
                        suffix = String.Format(Resource.Default, data.Comment.Default);
                }
                if (suffix == null && data.Default != null)
                {
                    if (data.Default.Value is bool) // Handle incorrect case boolean
                        suffix = String.Format(Resource.Default,
                            data.Default.Value.ToString().ToLower());
                    else
                        suffix = String.Format(Resource.Default, data.Default.Value);
                }
                if (suffix != null)
                {
                    if (comment[comment.Length - 1] != '.') comment += ".";
                    comment = String.Format("{0} {1}", comment, suffix);
                }

                // Output comment to writer
                this.WriteXmlComment(writer, String.Format("{0}: {1}",
                        data.Name, comment));

                // Ouput any enum constant supplmental comments
                if (supplementalComments != null)
                    foreach (string extraComment in supplementalComments)
                        this.WriteXmlComment(writer, extraComment);

            }
            #endregion

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes the actual comment to the XmlWriter wrapping onto multiple lines
        /// of text if required.
        /// </summary>
        /// <param name="writer">XmlWriter to output to.</param>
        /// <param name="comment">Zero or more length string to output</param>
        private void WriteXmlComment(XmlWriter writer, string comment)
        {

            while (comment != null && comment.Length > 0)
            {

                string substring;
                if (_MaxLength > 0)
                {

                    // Wrap the comment on multiple lines as required.
                    substring = comment.Substring(0, Math.Min(comment.Length, _MaxLength));

                    // If wrapping, back-up and wrap on last space (if possible)
                    if (substring.Length == _MaxLength)
                    {
                        int lastSpaceChar = substring.LastIndexOf(' ');
                        if (lastSpaceChar != -1)
                            substring = substring.Substring(0, lastSpaceChar);
                    }

                }
                else
                    substring = comment;

                // Remove text about to be output from the comment
                comment = comment.Substring(substring.Length).TrimStart();

                // More lines of comment required. Postfix and Prefix with CommentWrap
                if (comment.Length > 0)
                {
                    comment = String.Format("{0} {1}", Resource.CommentWrap, comment);
                    substring = String.Format("{0} {1}", substring, Resource.CommentWrap);
                }

                // Output comment to XmlWriter
                writer.WriteComment(String.Format(Resource.CommentPadding, substring));

            }

        }

        #endregion

    }
}
