/*
 
  OBS: USE NETFRAMEWORK 4.6
                  
 DESCRIÇÃO DO PROJETO - LIB PARA CRIAÇÃO DE CODIGO DE BARRAS EAN-13
 
     BarCode CODE =NEW BarCode();
     Bitmap bmp = CODE.DesenharCode("789916851001);
 
 O PROGRAMA DEVOLVE UM BITMAP QUE PODE SER DESENHADO EM QUALQUER SUPERFICIE.
   
                    
 SISTEMA CRIAÇÃO DE CODIGO DE BARRAS EAN-13
 AUTOR  -FABIO SOARES BRAGA 
 CONTATO-fsbraga01@hotmail.com TEL - 95970-5406
   

*/


using System.Text;
using System.Drawing;


namespace Codigo.Barra
{
    public class BarCode
    {
        string[] TabelaPaises = { "AAAAAA", "AABABB", "AABBAB", "AABBBA", "ABAABB", "ABBAAB", "ABBBAA", "ABABAB", "ABABBA", "ABBABA" };

        string[] TabelaA = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };

        string[] TabelaB = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };

        string[] TabelaC = { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };




        public Bitmap DesenharCode(string number)  //FUNÇAO O BITMAP COM CODIGO DESENHADO
        {
            number = number.Trim();

            Bitmap bmp = new Bitmap(200, 130);


            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, 125, 110);

            string binario = this.CreateCode(ref number);

            for (int i = 0; i < binario.Length; i++)
            {
                if (binario[i] == '1')
                {
                    if ((i <= 2) || (i >= 46 && i <= 49) || (i >= 92 && i <= 96))
                        g.DrawLine(new Pen(Color.Black), new Point(15 + i, 3), new Point(15 + i, 95));
                    else

                        g.DrawLine(new Pen(Color.Black), new Point(15 + i, 3), new Point(15 + i, 90));
                }
                else
                {
                    g.DrawLine(new Pen(Color.White), new Point(15 + i, 3), new Point(15 + i, 90));
                }
            }

            g.DrawString(number.Substring(0, 1), new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Black), new Point(3, 90));
            g.DrawString(number.Substring(1, 6), new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Black), new Point(15 + 1, 90));
            g.DrawString(number.Substring(7, 6), new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Black), new Point(63, 90));

            return bmp;
        }


        private string getBinario(int tab, int position) // FUNÇÃO DEVOLVE CODIGO BINARIO NAS TABELAS DE ACORDO COM A POSIÇÃO
        {
            switch (tab)
            {
                case 1: { return TabelaA[position]; }; break;
                case 2: { return TabelaB[position]; }; break;
                case 3: { return TabelaC[position]; }; break;
                default: return null;

            }

        }

        public string GetDigVerificador(string number) // DEVOLVEE O CODIGO VERIFICADOR
        {
            number = number.Trim();
            int dig = 0;
            int digDec = 0;


            for (int i = 1; i <= number.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    dig += int.Parse(number[i - 1].ToString()) * 3;
                }
                else
                {
                    dig += int.Parse(number[i - 1].ToString());
                }

            }

            while (digDec < dig)
                digDec += 10;

            dig = digDec - dig;

            return dig.ToString();
        }


        private string getPais(char pais)    // DEVOLVE O PAIS NA TABELA 2
        {

            return TabelaPaises[int.Parse(pais.ToString())];
        }


        public string CreateCode(ref string number)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("101");

            string pais = getPais(number[0]);

            for (int i = 1; i <= 6; i++)
            {
                if (pais[i - 1].Equals('A'))
                {
                    builder.Append(getBinario(1, int.Parse(number[i].ToString())));
                }
                else
                {

                    builder.Append(getBinario(2, int.Parse(number[i].ToString())));
                }

            }

            builder.Append("01010");


            number += GetDigVerificador(number);

            for (int i = 7; i <= 12; i++)
            {
                builder.Append(getBinario(3, int.Parse(number[i].ToString())));
            }
            
            builder.Append("101");

            return builder.ToString();
        }

    }
}
