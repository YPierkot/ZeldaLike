using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Text;
using UnityEditor;

public class DiscordWebHook : MonoBehaviour {
    //DISCORD LINK
    private static string baseDiscordLink = "https://discord.com/api/webhooks/";
    private static string defaultWebhookLink = "937758535608045578/a1GEnuVRV3kKrGIH_NbFovColFH6bD6Z95Ls0jqP8UaTc_RqKBe-Cz780tCWEj4M2kU4";
    private static string baseLink = baseDiscordLink + defaultWebhookLink;
    
    //DISCORD BOT DATA
    private static string defaultBotAgent = "Project Bug Report Bot";
    private static string defaultBotName = "Bug Report Bot";
    private static string defaultAvatar = "https://i.redd.it/8n6x4gk2pnr71.png";
    
    #if UNITY_EDITOR
    [MenuItem("Tools/Discord/Test Launcher Webhook")]
    public static void SendLauncherTestWebhook() {
        SendDiscordMessage("Test message for launcher update recieved successfully! :raised_hands:", "Launcher Update Bot");
    }
    
    [MenuItem("Tools/Discord/Test Bug Report Webhook")]
    public static void SendBugReportTestWebhook() {
        SendDiscordMessage("Test message for bug report recieved successfully! :raised_hands:", "Feedback Report Bot", "https://discord.com/api/webhooks/937761532727689336/FuJ-ztAamRLFq8Lxp-mpcZJms897AgzzEX_tYvbtmcqkZG7NSreMMV4BZi3d257RQM3g");
    }
    #endif
    
    /// <summary>
    /// Send a discord message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string SendDiscordMessage(string message, string overrideUsername = "" , string overrideWebhookLink = "", string overrideBotAvatar = "") {
        //Generate message Object
        Dictionary<string, object> postParameters = new Dictionary<string, object> {{"username", overrideUsername == "" ? defaultBotName : overrideUsername}, {"content", message}, {"avatar_url", overrideBotAvatar == "" ? defaultAvatar : overrideBotAvatar}};

        // Create request and receive response
        HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(overrideWebhookLink == "" ? baseLink : overrideWebhookLink, defaultBotAgent, postParameters);
        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()!);
        string fullResponse = responseReader.ReadToEnd();
        webResponse.Close();
        
        Debug.Log("The discord message has been successfully send!");
        
        return fullResponse;
    }
}

public static class FormUpload {
    private static readonly Encoding encoding = Encoding.UTF8;

    public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters) {
        string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

        string contentType = "multipart/form-data; boundary=" + formDataBoundary;

        byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

        return PostForm(postUrl, userAgent, contentType, formData);
    }

    private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData) {
        HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

        if (request == null) {
            Debug.LogWarning("request is not a http request");
            throw new NullReferenceException("request is not a http request");
        }

        // Set up the request properties.
        request.Method = "POST";
        request.ContentType = contentType;
        request.UserAgent = userAgent;
        request.CookieContainer = new CookieContainer();
        request.ContentLength = formData.Length;

        // Send the form data to the request.
        using (Stream requestStream = request.GetRequestStream()) {
            requestStream.Write(formData, 0, formData.Length);
            requestStream.Close();
        }

        return request.GetResponse() as HttpWebResponse;
    }

    private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary) {
        Stream formDataStream = new System.IO.MemoryStream();
        bool needsCLRF = false;

        foreach (var param in postParameters) {
            if (needsCLRF)
                formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

            needsCLRF = true;

            if (param.Value is FileParameter) {
                FileParameter fileToUpload = (FileParameter) param.Value;

                string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                    boundary,
                    param.Key,
                    fileToUpload.FileName ?? param.Key,
                    fileToUpload.ContentType ?? "application/octet-stream");

                formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
            }
            else {
                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
            }
        }

        // Add the end of the request.  Start with a newline
        string footer = "\r\n--" + boundary + "--\r\n";
        formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

        // Dump the Stream into a byte[]
        formDataStream.Position = 0;
        byte[] formData = new byte[formDataStream.Length];
        formDataStream.Read(formData, 0, formData.Length);
        formDataStream.Close();

        return formData;
    }

    private class FileParameter {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public FileParameter(byte[] file) : this(file, null) {
        }

        public FileParameter(byte[] file, string filename) : this(file, filename, null) {
        }

        public FileParameter(byte[] file, string filename, string contenttype) {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
}
