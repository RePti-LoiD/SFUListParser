﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using SFUListParser.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFUListParser.Scripts
{
    public class SFUHtmlListParser
    {
        public static async Task<List<Student>> ParseTableAsync(string link)
        {
            HttpClient httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(new Uri(link));

            IHtmlDocument angle = new HtmlParser().ParseDocument(html);

            int priorityPosition = 0;
            int parsedCount = 0;
            List<Student> students = new List<Student>();
            foreach (IElement table in angle.QuerySelectorAll("tr").ToList().Skip(13))
            {
                if (parsedCount >= ConfigHandler.ParseListLimit)
                    break;

                parsedCount++;

                var currentStudent = table.QuerySelectorAll("td");

                Debug.WriteLine((currentStudent[0].TextContent, currentStudent[1].TextContent));
                
                if (!string.IsNullOrEmpty(currentStudent[17].TextContent))
                    priorityPosition++;
                
                students.Add(new Student()
                {
                    Position = int.Parse(currentStudent[0].TextContent),
                    PriorityPosition = !string.IsNullOrEmpty(currentStudent[17].TextContent) ? priorityPosition : 0,
                    ID = currentStudent[1].TextContent,
                    AdditionalPoints = int.Parse(currentStudent[8].TextContent),
                    TotalPoints = int.Parse(currentStudent[7].TextContent),
                    Prioriry = int.Parse(currentStudent[2].TextContent),
                    IsHighestPriority = currentStudent[16].TextContent.Contains("1") ? true : false
                });
            }

            return students;
        }
    }
}