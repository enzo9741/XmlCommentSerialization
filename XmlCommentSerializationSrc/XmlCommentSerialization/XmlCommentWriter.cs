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
using System.IO;
using System.Text;
using System.Xml;

namespace XmlCommentSerialization
{
    public sealed class XmlCommentWriter : XmlWriter, IDisposable
    {

        // XmlWriter that does the serialization
        private XmlWriter _XmlWriter;

        // Alters XmlWriter Element output
        private bool _CommentMode;

        #region Public Properties

        /// <summary>
        /// Order attribute and element comments alphabetically
        /// </summary>
        public bool Alphabetize;

        /// <summary>
        /// Output comments from Type metadata even when XmlComment has not been specified
        /// </summary>
        public bool Metadata;

        /// <summary>
        /// Maximum comment length before wrapping. Zero means no wrap
        /// </summary>
        public int MaxLength;

        /// <summary>
        /// Output comments each time it appears in the XML
        /// </summary>
        public bool Repeat;

        #endregion

        #region Methods that alter XmlWriter element output behaviour

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (XmlCommentElement.Namespace.Equals(ns))
                _CommentMode = true; // Suppress Comment Start Element
            else
                _XmlWriter.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteEndElement()
        {
            if (_CommentMode)
                _CommentMode = false; // Suppress Comment End Element
            else
                _XmlWriter.WriteEndElement();
        }

        #endregion

        #region Regular (Wrapped) Pass-through XmlWriter Constructors

        public XmlCommentWriter(Stream output)
        {
            _XmlWriter = XmlWriter.Create(output);
        }

        public XmlCommentWriter(string outputFileName)
        {
            _XmlWriter = XmlWriter.Create(outputFileName);
        }

        public XmlCommentWriter(StringBuilder output)
        {
            _XmlWriter = XmlWriter.Create(output);
        }

        public XmlCommentWriter(TextWriter output)
        {
            _XmlWriter = XmlWriter.Create(output);
        }

        public XmlCommentWriter(XmlWriter output)
        {
            _XmlWriter = XmlWriter.Create(output);
        }

        public XmlCommentWriter(Stream output, XmlWriterSettings settings)
        {
            _XmlWriter = XmlWriter.Create(output, settings);
        }

        public XmlCommentWriter(string outputFileName, XmlWriterSettings settings)
        {
            _XmlWriter = XmlWriter.Create(outputFileName, settings);
        }

        public XmlCommentWriter(StringBuilder output, XmlWriterSettings settings)
        {
            _XmlWriter = XmlWriter.Create(output, settings);
        }

        public XmlCommentWriter(TextWriter output, XmlWriterSettings settings)
        {
            _XmlWriter = XmlWriter.Create(output, settings);
        }

        public XmlCommentWriter(XmlWriter output, XmlWriterSettings settings)
        {
            _XmlWriter = XmlWriter.Create(output, settings);
        }

        #endregion

        #region Regular (Wrapped) Pass-through XmlWriter Members

        public override void Flush()
        {
            _XmlWriter.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return _XmlWriter.LookupPrefix(ns);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            _XmlWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            _XmlWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            _XmlWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            _XmlWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            _XmlWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            _XmlWriter.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            _XmlWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            _XmlWriter.WriteEndDocument();
        }

        public override void WriteEntityRef(string name)
        {
            _XmlWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            _XmlWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            _XmlWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(string data)
        {
            _XmlWriter.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            _XmlWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _XmlWriter.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument(bool standalone)
        {
            _XmlWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            _XmlWriter.WriteStartDocument();
        }

        public override WriteState WriteState
        {
            get
            {
                return _XmlWriter.WriteState;
            }
        }

        public override void WriteString(string text)
        {
            _XmlWriter.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            _XmlWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            _XmlWriter.WriteWhitespace(ws);
        }

        #endregion

        #region IDisposable Implementation
        public new void Dispose()
        {
            // Must explicitly implement to release file resources
            if (_XmlWriter != null) _XmlWriter.Dispose();
        }
        #endregion

    }
}
