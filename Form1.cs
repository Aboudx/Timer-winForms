using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_Project
{
    public partial class frmTimer : Form
    {
        int Hours = 0, Minutes = 0, Seconds = 0;
        bool State = true;
        TimeSpan timeRemainder;
        double TimerInSeconds;
        public frmTimer()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            lbPercentage.Text = "0.00%";
            txtHours.Text = "0";
            txtMinutes.Text = "0";
            txtSeconds.Text = "0";
            btnStart.Enabled = true;
            progTimer.Maximum = 0;
            progTimer.Value = 0;
            State = true;

            txtHours.Enabled = true;
            txtMinutes.Enabled = true;
            txtSeconds.Enabled = true;

            Timer.Enabled = false;

            Hours = 0;
            Minutes = 0;    
            Seconds = 0;
        }

        private void frmTimer_Load(object sender, EventArgs e)
        {
            ResetForm();
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            PlayButtonSound();

        }

        private void txtHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (txtHours.Text == "0" && txtMinutes.Text == "0" && txtSeconds.Text == "0")
            {
                MessageBox.Show("You must do so before entering the time."
                    , "Warning.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PlayButtonSound();

            Timer.Enabled = false;
            btnPause.Enabled = false;
            btnStart.Enabled = true;
        }

        private void txtHours_TextChanged(object sender, EventArgs e)
        {
            TextBox Txt= ((TextBox)sender);
            if (String.IsNullOrEmpty(Txt.Text)) return;

            if(int.TryParse(Txt.Text,out int Value))
            {
                if (Value > 60)
                {
                    Txt.Text = "60";
                    Txt.SelectionStart = Txt.Text.Length;
                }
            }
        }

        private void PlayEndTimerSound()
        {
            SoundPlayer Sound = new SoundPlayer(Properties.Resources.TimerEnd_Sound);
            Sound.Play();
        }

        private void PlayButtonSound()
        {
            SoundPlayer Sound = new SoundPlayer(Properties.Resources.OK_FINAL_SOUND);
            Sound.Play();
        }

        private void SetStateIfTxtIsNull(TextBox txt)
        {
            if (String.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = "0";
            }
        }

        private void SetFirstTimerState()
        {
            Timer.Enabled = true;
            btnPause.Enabled = true;

            SetStateIfTxtIsNull(txtHours);
            SetStateIfTxtIsNull(txtMinutes);
            SetStateIfTxtIsNull(txtSeconds);

            Hours = Convert.ToInt32(txtHours.Text);
            Minutes = Convert.ToInt32(txtMinutes.Text);
            Seconds = Convert.ToInt32(txtSeconds.Text);
            btnStart.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtHours.Text == "0" && txtMinutes.Text == "0" && txtSeconds.Text == "0")
            {
                MessageBox.Show("You must do so before entering the time."
                    , "Warning.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            PlayButtonSound();

            SetFirstTimerState();

            txtHours.Enabled = false;
            txtMinutes.Enabled = false;
            txtSeconds.Enabled= false;
        } 

        private void CounterAndChangeTimer()
        {
            if (Seconds > 0)
            {
                Seconds--;
                txtSeconds.Text = (Seconds).ToString();
            }
            else
            {

                if (Minutes > 0)
                {
                    Seconds = 59;
                    Minutes--;
                    txtMinutes.Text = (Minutes).ToString();
                    txtSeconds.Text=(Seconds).ToString();
                }
                else if (Hours > 0)
                {
                    Seconds = 59;
                    Minutes = 59;
                    Hours--;
                    txtHours.Text = (Hours).ToString();
                    txtMinutes.Text = (Minutes).ToString();
                    txtSeconds.Text = (Seconds).ToString();
                }
                else
                {
                    txtSeconds.Text = (Seconds).ToString();
                    Timer.Enabled = false;

                    if (txtHours.Text == "0" && txtMinutes.Text == "0" && txtSeconds.Text == "0")
                        PlayEndTimerSound();
                }
            }
        }

        private void ChangeProgressParState(double ElapsedSeconds)
        {
            if(State)
           {  
                timeRemainder = new TimeSpan(Hours, Minutes, Seconds);

                TimerInSeconds= timeRemainder.TotalSeconds;
                progTimer.Maximum = Convert.ToInt32(TimerInSeconds);
                State = false;
            }

            if (progTimer.Value < progTimer.Maximum)
            {
                progTimer.Value += 1; 

            }

            lbPercentage.Text = (((TimerInSeconds - ElapsedSeconds) / TimerInSeconds) * 100).ToString("F2") + "%";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ElapsedTime = new TimeSpan(Hours, Minutes, Seconds);
            ChangeProgressParState(ElapsedTime.TotalSeconds);
            CounterAndChangeTimer();
        }
    }
}
