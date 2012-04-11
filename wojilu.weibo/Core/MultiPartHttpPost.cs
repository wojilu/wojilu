using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using wojilu.weibo.Common;
using wojilu.weibo.Core.Sina;

namespace wojilu.weibo.Core
{
    /// <summary>
    /// Performs a multi-part (multipart/form-data) Http-Post request to the resource identified by the uri with the specified PartFields.
    /// </summary>
    public class MultiPartHttpPost : HttpPost
    {
        private IList<MultiPartField> partFields = new Collection<MultiPartField>();
        private const string PartHeaderPattern = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n"; //Content-Type: text/plain; charset=US-ASCII\r\nContent-Transfer-Encoding: 8bit\r\n
        private const string FilePartHeaderPattern = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3};\r\n";//Content-Transfer-Encoding: binary\r\n
        private string boundary;

        // Must use this encoding.
        private const string EncodingName = "iso-8859-1";
        private const string CRLF = "\r\n"; // Line separator in multipart/form-data data post. See RFC 1867
        
        /// <summary>
        /// Initializes a new instance of <see cref="MultiPartHttpPost"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        public MultiPartHttpPost(string uri)
            : base(uri)
        {
            boundary = "---------------------------7db2b61c40302"; // TODO: Random it
            base.ContentType = string.Format(SinaConstants.PostMultiPartContentType, boundary);
        }

        /// <summary>
        /// Gets the data to post.
        /// </summary>
        public sealed override string PostData
        {
            get
            {
                return ConstructPostBody();
            }
            set
            {
                throw new NotSupportedException("Not supported. Please use PartFields property to convey data to post.");                
            }
        }

        /// <summary>
        /// Gets the multi-part fields.
        /// </summary>
        public IList<MultiPartField> PartFields
        {
            get
            {
                return partFields;
            }
        }

        private string ConstructPostBody()
        {
            var postBodyBuilder = new StringBuilder();
            postBodyBuilder.AppendLine();

            foreach (var field in PartFields)
            {
                var fieldName = field.Name; // TODO: Perform RFC1522 Encoding agaisnt field.Name if applicable.
                var partHeader = string.Empty;
                if (string.IsNullOrEmpty(field.FilePath))
                {
                    partHeader = string.Format(PartHeaderPattern, boundary, fieldName);
                }
                else
                {
                    var fileContentType = GetFileContentType(field.FilePath);
                    partHeader = string.Format(FilePartHeaderPattern, boundary, fieldName, Path.GetFileName(field.FilePath), fileContentType);

                    var fileData = File.ReadAllBytes(field.FilePath);
                    field.Value = Encoding.GetEncoding(EncodingName).GetString(fileData);
                }

                postBodyBuilder.AppendLine(partHeader);
                postBodyBuilder.AppendLine(field.Value);
            }

            // End tag of muiti-part.
            postBodyBuilder.AppendFormat("--{0}--\r\n", boundary);

            return postBodyBuilder.ToString();
        }

        /// <summary>
        /// See <see cref="HttpPost.WriteBody"/>
        /// </summary>
        protected override void WriteBody(Stream reqStream)
        {
            var postData = PostData;
            if (!string.IsNullOrEmpty(postData))
            {
                var dataBytes = Encoding.GetEncoding(EncodingName).GetBytes(postData);
                reqStream.Write(dataBytes, 0, dataBytes.Length);
            }
        }

        private string GetFileContentType(string fileName)
        {
            var contentType = string.Empty;
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".png":
                case ".gif":
                case ".jpeg":
                    contentType = "image/" + ext.Remove(0,1);
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }
    }

}
