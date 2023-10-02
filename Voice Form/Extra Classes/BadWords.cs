using Newtonsoft.Json;

namespace Voice_Form.Extra_Classes
{
    public class BadWords
    {
        public BadWordsRootObject badwordslist { get; set; }
        public static BadWordsRootObject ReadBadWords(string path)
        {
            var jsonContent = System.IO.File.ReadAllText(path);
            var badwords = JsonConvert.DeserializeObject<BadWordsRootObject>(jsonContent);
            return badwords;
        }

        public static bool CheckBadWords(string cumle,string path)
        {
            var kotuKelimeler = ReadBadWords(path);
            var kelimeler = cumle.Split(' ');

            foreach (var kelime in kelimeler)
            {
                foreach (var badword in kotuKelimeler.RECORDS)
                {
                    if (badword.word==kelime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
