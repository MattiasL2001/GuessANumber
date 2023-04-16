using static System.Formats.Asn1.AsnWriter;

namespace GuessANumberTests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void Create_File_If_Not_Exist_Then_Check_If_Exists()
        {
            string path = "C:\\Users\\Matti\\OneDrive\\Skrivbord\\Highscore.txt";

            if (!File.Exists(path)) { File.Create(path); }

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Write_To_File_Then_Read_File_Content()
        {
            string filePath = "C:\\Users\\Matti\\OneDrive\\Skrivbord\\Highscore.txt";
            string stringBuilder = "";
            stringBuilder += "{\n   ";
            stringBuilder += "5,\n   ";
            stringBuilder += "Mattias,\n   ";
            stringBuilder += "2023-04-16 19:37:48,\n";
            stringBuilder += "}";
            File.WriteAllText(filePath, stringBuilder);
            string content = File.ReadAllText(filePath);
            content = content.Replace("},{", "");
            content = content.Replace("}", "");
            content = content.Replace("\n", "");
            content = content.Replace("{", "");
            content = content.Replace("   ", "");
            content = content.Trim();
            string[] elements = content.Split(",");
            int objCount = elements.Length / 3;

            Assert.AreEqual("5", elements[0]);
        }
    }
}