using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Security.Cryptography;

namespace GameOptions
{
    public class FileProcessing
    {
        private static string fileName = @".\LouGames.xml";
        private static string encPassword = "RottenCheese";

        /* Minesweeper */
        public static Tuple<string[], int[]> LoadMinesweeper()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);
                var times = new int[3];
                var names = new string[3];

                XmlNode child = xdoc.SelectSingleNode("//LouGames/Minesweeper/Times/Beginner");
                times[0] = Convert.ToInt32(DecryptString(child.ChildNodes[0].InnerText, encPassword));
                names[0] = DecryptString(child.ChildNodes[1].InnerText, encPassword);

                child = xdoc.SelectSingleNode("//LouGames/Minesweeper/Times/Intermediate");
                times[1] = Convert.ToInt32(DecryptString(child.ChildNodes[0].InnerText, encPassword));
                names[1] = DecryptString(child.ChildNodes[1].InnerText, encPassword);

                child = xdoc.SelectSingleNode("//LouGames/Minesweeper/Times/Expert");
                times[2] = Convert.ToInt32(DecryptString(child.ChildNodes[0].InnerText, encPassword));
                names[2] = DecryptString(child.ChildNodes[1].InnerText, encPassword);
                return new Tuple<string[], int[]>(names, times);
            }
            catch
            {
                ResetScoresFile();
                return LoadMinesweeper();
            }
        }

        public static void SaveMinesweeper(string[] names, int[] times)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList nodeList = doc.GetElementsByTagName("Minesweeper");
                XmlNode node = nodeList.Item(0).FirstChild;
                XmlNode child, innerChild;
                node.RemoveAll();

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Beginner", ""));

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
                innerChild.InnerText = EncryptString(times[0].ToString(), encPassword);

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                innerChild.InnerText = EncryptString(names[0], encPassword);

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Intermediate", ""));

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
                innerChild.InnerText = EncryptString(times[1].ToString(), encPassword);

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                innerChild.InnerText = EncryptString(names[1], encPassword);

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Expert", ""));

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
                innerChild.InnerText = EncryptString(times[2].ToString(), encPassword);

                innerChild = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                innerChild.InnerText = EncryptString(names[2], encPassword);

                doc.Save(fileName);
            }
            catch
            {
                ResetScoresFile();
            }
        }

        /* Hearts */
        public static void LoadHearts(ref string player, ref string comp1, ref string comp2, ref string comp3)
        {
            try
            {
                player = string.Empty;
                comp1 = string.Empty;
                comp2 = string.Empty;
                comp3 = string.Empty;

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);

                XmlNode child = xdoc.SelectSingleNode("//LouGames/Hearts/Names/Computer1");
                comp1 = DecryptString(child.InnerText, encPassword);

                child = xdoc.SelectSingleNode("//LouGames/Hearts/Names/Computer2");
                comp2 = DecryptString(child.InnerText, encPassword);

                child = xdoc.SelectSingleNode("//LouGames/Hearts/Names/Computer3");
                comp3 = DecryptString(child.InnerText, encPassword);
            }
            catch
            {
                ResetScoresFile();
                LoadHearts(ref player, ref comp1, ref comp2, ref comp3);
            }
        }

        public static void SaveHearts(string comp1, string comp2, string comp3)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList nodeList = doc.GetElementsByTagName("Hearts");
                XmlNode node = nodeList.Item(0).FirstChild;
                XmlNode child;
                node.RemoveAll();

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer1", ""));
                child.InnerText = EncryptString(comp1, encPassword);

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer2", ""));
                child.InnerText = EncryptString(comp2, encPassword);

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer3", ""));
                child.InnerText = EncryptString(comp3, encPassword);

                doc.Save(fileName);
            }
            catch
            {
                ResetScoresFile();
            }
        }

        /* FreeCell */
        public static void LoadFreeCell(ref bool allowRightClick, ref int gamesPlayed, ref int gamesWon, ref int gamesForfiet, ref int gamesLost)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);
                XmlNode child = xdoc.SelectSingleNode("//LouGames/FreeCell/Options/AllowRightClick");
                allowRightClick = Convert.ToBoolean(DecryptString(child.InnerText, encPassword));

                child = xdoc.SelectSingleNode("//LouGames/FreeCell/Options/GamesPlayed");
                gamesPlayed = Convert.ToInt32(DecryptString(child.InnerText, encPassword));

                child = xdoc.SelectSingleNode("//LouGames/FreeCell/Options/GamesWon");
                gamesWon = Convert.ToInt32(DecryptString(child.InnerText, encPassword));

                child = xdoc.SelectSingleNode("//LouGames/FreeCell/Options/GamesLost");
                gamesLost = Convert.ToInt32(DecryptString(child.InnerText, encPassword));

                child = xdoc.SelectSingleNode("//LouGames/FreeCell/Options/GamesForfiet");
                gamesForfiet = Convert.ToInt32(DecryptString(child.InnerText, encPassword));
            }
            catch
            {
                ResetScoresFile();
                LoadFreeCell(ref allowRightClick, ref gamesPlayed, ref gamesWon, ref gamesForfiet, ref gamesLost);
            }
        }

        public static void SaveFreeCell(bool allowRightClick, int gamesPlayed, int gamesWon, int gamesForfiet, int gamesLost)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);
                XmlNodeList nodeList = xdoc.GetElementsByTagName("FreeCell");
                XmlNode node = nodeList.Item(0).FirstChild;
                XmlNode child;
                node.RemoveAll();

                child = node.AppendChild(xdoc.CreateNode(XmlNodeType.Element, "AllowRightClick", ""));
                child.InnerText = EncryptString(allowRightClick.ToString(), encPassword);

                child = node.AppendChild(xdoc.CreateNode(XmlNodeType.Element, "GamesPlayed", ""));
                child.InnerText = EncryptString(gamesPlayed.ToString(), encPassword);

                child = node.AppendChild(xdoc.CreateNode(XmlNodeType.Element, "GamesWon", ""));
                child.InnerText = EncryptString(gamesWon.ToString(), encPassword);

                child = node.AppendChild(xdoc.CreateNode(XmlNodeType.Element, "GamesLost", ""));
                child.InnerText = EncryptString(gamesLost.ToString(), encPassword);

                child = node.AppendChild(xdoc.CreateNode(XmlNodeType.Element, "GamesForfiet", ""));
                child.InnerText = EncryptString(gamesForfiet.ToString(), encPassword);

                xdoc.Save(fileName);
            }
            catch
            {
                ResetScoresFile();
            }
        }

        /* Bejeweled */
        public static void LoadBejeweled(ref string[] topTenNames, ref int[] topTenScores)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);

                XmlNodeList simple = xdoc.SelectNodes("//LouGames/Bejeweled/Scores/SimpleScores/Player");
                XmlNodeList timed = xdoc.SelectNodes("//LouGames/Bejeweled/Scores/TimedScores/Player");

                if (simple == null || simple.Count == 0 || timed == null || timed.Count == 0)
                    throw new ArgumentNullException();

                for (int i = 0; i < simple.Count; i++)
                {
                    topTenNames[i] = DecryptString(simple[i].ChildNodes[0].InnerText, encPassword);
                    topTenScores[i] = Convert.ToInt32(DecryptString(simple[i].ChildNodes[1].InnerText, encPassword));
                }

                for (int i = 0; i < timed.Count; i++)
                {
                    topTenNames[i + 10] = DecryptString(simple[i].ChildNodes[0].InnerText, encPassword);
                    topTenScores[i + 10] = Convert.ToInt32(DecryptString(simple[i].ChildNodes[1].InnerText, encPassword));
                }
            }
            catch
            {
                ResetScoresFile();
                LoadBejeweled(ref topTenNames, ref topTenScores);
            }
        }

        public static void SaveBejeweled(string[] topTenNames, int[] topTenScores)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList nodeList = doc.GetElementsByTagName("Bejeweled");
                XmlNode node = nodeList.Item(0).FirstChild;
                XmlNode child, innerChild1, innerChild2;
                node.RemoveAll();

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "SimpleScores", ""));

                for (int i = 0; i < 10; i++)
                {
                    innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Player", ""));

                    innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                    innerChild2.InnerText = EncryptString(topTenNames[i], encPassword);

                    innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Score", ""));
                    innerChild2.InnerText = EncryptString(topTenScores[i].ToString(), encPassword);

                }

                child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "TimedScores", ""));

                for (int i = 10; i < 20; i++)
                {
                    innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Player", ""));

                    innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                    innerChild2.InnerText = EncryptString(topTenNames[i], encPassword);

                    innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Score", ""));
                    innerChild2.InnerText = EncryptString(topTenScores[i].ToString(), encPassword);
                }

                doc.Save(fileName);
            }
            catch
            {
                ResetScoresFile();
            }
        }

        /* Create New File */
        private static void ResetScoresFile()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlNode rootNode = doc.CreateNode(XmlNodeType.Element, "LouGames", "");
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
            doc.AppendChild(rootNode);


            //Bejeweled
            XmlNode node = doc.ChildNodes[1].AppendChild(doc.CreateNode(XmlNodeType.Element, "Bejeweled", ""));
            XmlNode child, innerChild1, innerChild2;

            node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Scores", ""));
            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "SimpleScores", ""));
            int score = 8000;
            for (int i = 0; i < 10; i++)
            {
                innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Player", ""));

                innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                innerChild2.InnerText = EncryptString("Bob McBoberson", encPassword);

                innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Score", ""));
                innerChild2.InnerText = EncryptString(score.ToString(), encPassword);
                score = score / 2;
            }

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "TimedScores", ""));
            score = 8000;
            for (int i = 10; i < 20; i++)
            {
                innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Player", ""));

                innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                innerChild2.InnerText = EncryptString("Bob McBoberson", encPassword);

                innerChild2 = innerChild1.AppendChild(doc.CreateNode(XmlNodeType.Element, "Score", ""));
                innerChild2.InnerText = EncryptString(score.ToString(), encPassword);
                score = score / 2;
            } 

            //FreeCell
            node = doc.ChildNodes[1].AppendChild(doc.CreateNode(XmlNodeType.Element, "FreeCell", ""));
            node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Options", ""));
            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "AllowRightClick", ""));
            child.InnerText = EncryptString("True", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "GamesPlayed", ""));
            child.InnerText = EncryptString("0", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "GamesWon", ""));
            child.InnerText = EncryptString("0", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "GamesLost", ""));
            child.InnerText = EncryptString("0", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "GamesForfiet", ""));
            child.InnerText = EncryptString("0", encPassword);

            //Hearts
            node = doc.ChildNodes[1].AppendChild(doc.CreateNode(XmlNodeType.Element, "Hearts", ""));
            node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Names", ""));
            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer1", ""));
            child.InnerText = EncryptString("Player1", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer2", ""));
            child.InnerText = EncryptString("Player2", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Computer3", ""));
            child.InnerText = EncryptString("Player3", encPassword);

            //Minesweeper
            node = doc.ChildNodes[1].AppendChild(doc.CreateNode(XmlNodeType.Element, "Minesweeper", ""));
            node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Times", ""));
            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Beginner", ""));

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
            innerChild1.InnerText = EncryptString("9999", encPassword);

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
            innerChild1.InnerText = EncryptString("Anonymous", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Intermediate", ""));

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
            innerChild1.InnerText = EncryptString("9999", encPassword);

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
            innerChild1.InnerText = EncryptString("Anonymous", encPassword);

            child = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Expert", ""));

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Time", ""));
            innerChild1.InnerText = EncryptString("9999", encPassword);

            innerChild1 = child.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
            innerChild1.InnerText = EncryptString("Anonymous", encPassword);

            doc.Save(fileName);
        }

        /* Encryption */
        private static string EncryptString(string encString, string password)
        {
            MemoryStream ms = new MemoryStream();
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(encString);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 
                            0x20, 0x4d, 0x65, 0x64, 0x76, 
                            0x65, 0x64, 0x65, 0x76 });

            byte[] Key = pdb.GetBytes(32);
            byte[] IV = pdb.GetBytes(16);

            Rijndael encryptor = RijndaelManaged.Create();

            encryptor.Key = Key;
            encryptor.IV = IV;
            encryptor.Padding = PaddingMode.PKCS7;

            CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            encString = Convert.ToBase64String(encryptedData);
            return encString;
        }

        private static string DecryptString(string decString, string password)
        {
            MemoryStream ms = new MemoryStream();
            byte[] clearBytes = Convert.FromBase64String(decString);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 
                            0x20, 0x4d, 0x65, 0x64, 0x76, 
                            0x65, 0x64, 0x65, 0x76 });

            byte[] Key = pdb.GetBytes(32);
            byte[] IV = pdb.GetBytes(16);

            Rijndael encryptor = RijndaelManaged.Create();

            encryptor.Key = Key;
            encryptor.IV = IV;
            encryptor.Padding = PaddingMode.PKCS7;

            CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            decString = System.Text.Encoding.Unicode.GetString(decryptedData);
            return decString;
        }
    }
}
