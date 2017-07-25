using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using WebApi.Models.Response;

namespace WebApi
{
    public class Utils
    {
        private static string shorturl_chars_lcase = "abcdefgijkmnopqrstwxyz";
        private static string shorturl_chars_ucase = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string shorturl_chars_numeric = "23456789";

        public static string UniqueShortUrl()
        {
            string ShortUrl = RandomCharacters();

            //SqlServer sql_ = new SqlServer();

            string sql = "SELECT COUNT(*) FROM tblUrl WHERE ShortUrl = @ShortUrl";
            Params p = new Params();
            p.Add("@ShortUrl", ShortUrl);
            int url_count = Int32.Parse(SqlServer.Scalar(sql, p));

            if (url_count == 0)
            {
                return ShortUrl;
            }
            else
            {
                return RandomCharacters();
            }
        }

        public static string Clean(string url)
        {
            string filter = @"((https?):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";
            Regex rx = new Regex(filter);

            return url;
        }

        public static string PublicShortUrl(string ShortUrl)
        {
            return "http://" + ConfigurationManager.AppSettings["DomainName"].ToString() + "/" + ShortUrl;
        }

        public static string InternalShortUrl(string ShortUrl)
        {
            return ShortUrl.Replace("http://" + ConfigurationManager.AppSettings["DomainName"].ToString() + "/", "");
        }

        public int AddUrlToDatabase(Url oShortUrl)
        {
            SqlServer sql_ = new SqlServer();

            string sql = "INSERT INTO tblUrl (ShortUrl, Url, UserId) VALUES (@ShortUrl, @real_url, @created_by)";
            Params p = new Params();
            p.Add("@ShortUrl", oShortUrl.ShortenedUrl);
            p.Add("@real_url", Clean(oShortUrl.RealUrl));
            p.Add("@created_by", oShortUrl.CreatedBy);
            //p.Add("@create_date", oShortUrl.CreateDate);
            return SqlServer.Execute(sql, p, true);
        }

        public void AddUrlToLogDatabase(int id)
        {
            SqlServer sql_ = new SqlServer();

            string sql = "INSERT INTO tblUrlLog (Id) VALUES (@Id)";
            Params p = new Params();
            p.Add("@Id", id);

            //p.Add("@create_date", oShortUrl.CreateDate);
            SqlServer.Execute(sql, p);
        }

        public void AddUser(string id)
        {
            SqlServer sql_ = new SqlServer();

            string sql = "INSERT INTO tblUsuario (UserId) VALUES (@Id)";
            Params p = new Params();
            p.Add("@Id", id);

            SqlServer.Execute(sql, p);
        }

        public void DeleteUser(string id)
        {
            SqlServer sql_ = new SqlServer();

            string sql = "DELETE FROM tblUsuario Where UserId = @Id";
            Params p = new Params();
            p.Add("@Id", id);

            //p.Add("@create_date", oShortUrl.CreateDate);
            SqlServer.Execute(sql, p);
        }

        public void Deleteurl(string id)
        {
            SqlServer sql_ = new SqlServer();

            string sql = "DELETE FROM tblUrl Where id = @Id";
            Params p = new Params();
            p.Add("@Id", id);

            //p.Add("@create_date", oShortUrl.CreateDate);
            SqlServer.Execute(sql, p);
        }

        public Url RetrieveUrlFromDatabase(int id)
        {
            Url oShortUrl = new Url();
            //oShortUrl.ShortenedUrl = internal_url;

            SqlServer sql_ = new SqlServer();

            string sql = "SELECT * FROM tblUrl WHERE Id = @Id";
            Params p = new Params();
            p.Add("@Id", id);
            DataTable dt = SqlServer.Recordset(sql, p);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                //oShortUrl.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                oShortUrl.id = int.Parse(row["Id"].ToString());
                oShortUrl.CreatedBy = row["UserId"].ToString();
                oShortUrl.RealUrl = row["Url"].ToString();
            }

            AddUrlToLogDatabase(id);

            return oShortUrl;
        }

        public UserUrlStats getStatsByUser(string id = "")
        {
            UserUrlStats stats = new UserUrlStats();
            Url oShortUrl = new Url();

            List<Url> listUrls = new List<Url>();

            SqlServer sql_ = new SqlServer();


            string sql = "Select Top 10 url.Id, url.ShortUrl, url.Url, url.UserId, Count(*) as 'Hits'  From tblUrlLog log "
                          + " inner join tblUrl url on url.Id = log.Id "
                          + " Where url.UserId = @id "
                          + " Group By url.Id, url.ShortUrl, url.Url, url.UserId ";

            Params p = new Params();
            p.Add("@Id", id);
            DataTable dt = SqlServer.Recordset(sql, p);

            foreach (DataRow row in dt.Rows)
            {
                oShortUrl = new Url();

                oShortUrl.id = int.Parse(row["Id"].ToString());
                oShortUrl.hits = int.Parse(row["Hits"].ToString());
                oShortUrl.RealUrl = row["Url"].ToString();
                oShortUrl.ShortenedUrl = row["ShortUrl"].ToString();

                listUrls.Add(oShortUrl);

                stats.userid = id;
            }

            stats.UrlUserStats.hits = getHitsGlobal();
            stats.UrlUserStats.urlCount = getUrlsCountGlobal();
            stats.UrlUserStats.topUrls = listUrls;

            return stats;
        }

        public int getUserById(string id)
        {
            Url oShortUrl = new Url();

            SqlServer sql_ = new SqlServer();

            string sql = "Select Count (*) as Existe From tblUsuario Where UserId=@Id";

            Params p = new Params();
            p.Add("@Id", id);
            DataTable dt = SqlServer.Recordset(sql,p);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return int.Parse(row["Existe"].ToString());
            }
            else
                return 0;
        }

        public int getHitsGlobal()
        {
            Url oShortUrl = new Url();

            SqlServer sql_ = new SqlServer();

            string sql = "Select Count (*) as Hits From tblUrlLog";

            DataTable dt = SqlServer.Recordset(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return int.Parse(row["Hits"].ToString());
            }
            else
                return 0;
        }

        public int getUrlsCountGlobal()
        {
            Url oShortUrl = new Url();

            SqlServer sql_ = new SqlServer();

            string sql = "Select Count (*) as UrlCount From tblUrl";

            DataTable dt = SqlServer.Recordset(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return int.Parse(row["UrlCount"].ToString());
            }
            else
                return 0;
        }

        public UrlStats getStats(int id = 0)
        {
            UrlStats stats = new UrlStats();
            Url oShortUrl = new Url();

            List<Url> listUrls = new List<Url>();

            SqlServer sql_ = new SqlServer();

            string sql = String.Empty;

            DataTable dt;

            if (id == 0)
            {
                sql = "Select Top 10 url.Id, url.ShortUrl, url.Url, url.UserId, Count(*) as 'Hits'  From tblUrlLog log"
                              + " inner join tblUrl url on url.Id = log.Id"
                              + " Group By url.Id, url.ShortUrl, url.Url, url.UserId"
                              +"  Order By 5 Desc";

                dt = SqlServer.Recordset(sql);

                stats.hits = getHitsGlobal();
                stats.urlCount = getUrlsCountGlobal();
            }
            else
            {
                sql = "Select url.Id, url.ShortUrl, url.Url, url.UserId, Count(*) as 'Hits'  From tblUrlLog log"
                          + " inner join tblUrl url on url.Id = log.Id "
                          + " Where url.id = @Id "
                          + " Group By url.Id, url.ShortUrl, url.Url, url.UserId "
                          + "  Order By 5 Desc";

                Params p = new Params();
                p.Add("@Id", id);
                dt = SqlServer.Recordset(sql,p);

                stats.hits = null;
                stats.urlCount = null;
            }

            foreach (DataRow row in dt.Rows)
            {
                oShortUrl = new Url();

                oShortUrl.id = int.Parse(row["Id"].ToString());
                oShortUrl.hits = int.Parse(row["Hits"].ToString());
                oShortUrl.RealUrl = row["Url"].ToString();
                oShortUrl.ShortenedUrl = row["ShortUrl"].ToString();

                listUrls.Add(oShortUrl);
            }

            stats.topUrls = listUrls;

            return stats;
        }

        public static bool HasValue(object o)
        {
            if (o == null)
            {
                return false;
            }

            if (o == System.DBNull.Value)
            {
                return false;
            }

            if (o is String)
            {
                if (((String)o).Trim() == String.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        public static string RandomCharacters()
        {
            // Create a local array containing supported short-url characters
            // grouped by types.
            char[][] charGroups = new char[][] 
            {
                shorturl_chars_lcase.ToCharArray(),
                shorturl_chars_ucase.ToCharArray(),
                shorturl_chars_numeric.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold short-url characters.
            char[] ShortUrl = null;

            // Allocate appropriate memory for the short-url.
            ShortUrl = new char[random.Next(5, 5)];

            // Index of the next character to be added to short-url.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate short-url characters one at a time.
            for (int i = 0; i < ShortUrl.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the short-url.
                ShortUrl[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(ShortUrl);
        }
    }
}