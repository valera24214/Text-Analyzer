using System.Security.Cryptography.Xml;
using WinFormsApp2.EventsArguments;
using WinFormsApp2.TextProcessors;
using WinFormsApp2.TextProvider;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label3.Visible = false;
            textBox2.Visible = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            WikkiTextProvider textProvider = new WikkiTextProvider();
            ITextProcessor processor;

            button1.Enabled = false;
            textBox1.Clear();

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    { 
                        processor = new ExtremumWordProcessor(ExtremumMode.Maximum);
                        break;
                    }
                case 1:
                    {
                        processor = new ExtremumWordProcessor(ExtremumMode.Minimum);
                        break;
                    }
                case 2:
                    {
                        if (!string.IsNullOrEmpty(textBox2.Text) && char.IsLetter(textBox2.Text[0]) )
                            processor = new SettedCharCountProcessor(textBox2.Text[0]);
                        else
                        {
                            textBox1.AppendText("There is no letter in textBox" + Environment.NewLine);
                            return;
                        }

                        break;
                    }
                default:
                    {
                        MessageBox.Show("Invalid comboBox value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            }

            int pageCount = 0;
            processor.OnProcessed += (s, e) =>
            {
                textBox1.Invoke(() =>
                {
                    pageCount++;
                    textBox1.AppendText($"{DateTime.UtcNow}; Страниц обработано: {pageCount}; ");
                    if (e is WordResultArgs wordArgs)
                    {
                        textBox1.AppendText($"{wordArgs.description}: {wordArgs.targetWord}{Environment.NewLine}");
                    }
                    else if (e is CountResultArgs countArgs)
                    {
                        textBox1.AppendText($"{countArgs.description}: {countArgs.count}{Environment.NewLine}");
                    }
                });
            };

            TextAnalysisCoordinator coordinator = new TextAnalysisCoordinator(textProvider, processor, (int)numericUpDown2.Value);
            var result = await coordinator.Analyze((int)numericUpDown1.Value);
            textBox1.AppendText($"Итог: {result}");

            button1.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2) 
            {
                label3.Visible = true;
                textBox2.Visible = true;
            }
            else 
            {
                label3.Visible = false;
                textBox2.Visible = false;
            }
        }
    }
}
