using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace V_Shutdown
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] Command = Properties.Resources.Commands.Split(',');
            try
            {
                var Engine = new SpeechRecognitionEngine();
                Engine.SetInputToDefaultAudioDevice();

                var choices = new Choices(Command);
                var GBuilder = new GrammarBuilder();
                GBuilder.Append(choices);
                var grammar = new Grammar(GBuilder);
                Engine.LoadGrammar(grammar);

                //エンジンモード
                Engine.RecognizeAsync(RecognizeMode.Multiple);

                //認識イベントハンドラ
                Engine.SpeechRecognized += Engie_Recognized;
            }
            catch
            {
                MessageBox.Show("Engine Error" + Environment.NewLine + "マイクが有効になっていないかコマンドデータが破損しています");
            }
        }

        private void Engie_Recognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                if (e.Result.Confidence >= 0.300000)
                {
                    if (e.Result.Text == "Shutdown now") { Shutdown(); }
                    else if (e.Result.Text == "Reboot now") { reboot(); }
                    else if (e.Result.Text == "System exit") { exit(); }
                }
            }
            catch
            {
                MessageBox.Show("Engine Error");
                Application.Exit();
            }
        }

        private void Shutdown()
        {
            DialogResult result = MessageBox.Show("シャットダウンしますか?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == result)
            {
                var process = new ProcessStartInfo
                {
                    FileName = "shutdown.exe",
                    Arguments = "-s -t 0",
                    CreateNoWindow = true
                };
                Process.Start(process);
                Application.Exit();
            }
        }

        private void reboot()
        {
            DialogResult result = MessageBox.Show("再起動しますか?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == result)
            {
                var process = new ProcessStartInfo
                {
                    FileName = "shutdown.exe",
                    Arguments = "-r -t 0",
                    CreateNoWindow = true
                };
                Process.Start(process);
                Application.Exit();
            }
        }

        private void exit()
        {
            DialogResult result = MessageBox.Show("終了しますか?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == result) { Application.Exit(); }
        }
    }
}
