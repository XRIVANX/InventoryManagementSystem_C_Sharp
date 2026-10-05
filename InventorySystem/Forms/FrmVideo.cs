using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MediaElement = System.Windows.Controls.MediaElement;
using MediaState = System.Windows.Controls.MediaState;
using Stretch = System.Windows.Media.Stretch;
using InventorySystem.Helpers;
using Button = System.Windows.Forms.Button;
using Orientation = System.Windows.Forms.Orientation;

namespace InventorySystem.Forms
{
    public sealed class FrmVideo : Form
    {
        private readonly MediaElement player;
        private readonly Button play;
        private readonly Button stop;
        private readonly TrackBar timeline;
        private readonly Label status;
        private readonly Timer timer;
        private bool ready;
        private bool playing;
        private bool seeking;
        private bool ended;

        public FrmVideo()
        {
            Text = "Video";
            ClientSize = new Size(1000, 700);
            BackColor = ModernTheme.Canvas;
            AutoScaleMode = AutoScaleMode.Dpi;
            Branding.ApplyIcon(this);

            player = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Close,
                Stretch = Stretch.Uniform,
                Volume = 0.7
            };
            var host = new ElementHost { Dock = DockStyle.Fill, BackColor = Color.Black, Child = player };
            var controls = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom, Height = 58, Padding = new Padding(12, 8, 12, 8),
                BackColor = Color.White, WrapContents = false, AutoScroll = true
            };
            play = new Button { Text = "Play", Width = 90, Height = 34, Enabled = false };
            stop = new Button { Text = "Stop", Width = 90, Height = 34, Enabled = false };
            ModernTheme.Button(play, true);
            ModernTheme.Button(stop);
            var volumeLabel = ModernTheme.Label("Volume", 9, ModernTheme.Muted);
            volumeLabel.Margin = new Padding(18, 9, 4, 0);
            var volume = new TrackBar { Minimum = 0, Maximum = 100, Value = 70, Width = 120, Height = 34, TickStyle = TickStyle.None };
            volume.ValueChanged += (s, e) => player.Volume = volume.Value / 100.0;
            var close = new Button { Text = "Close", Width = 90, Height = 34 };
            ModernTheme.Button(close);
            close.Click += (s, e) => Close();
            controls.Controls.AddRange(new Control[] { play, stop, volumeLabel, volume, close });

            status = new Label
            {
                Text = "Loading video...", Dock = DockStyle.Bottom, Height = 30,
                ForeColor = ModernTheme.Muted, Font = ModernTheme.Font(9),
                TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(16, 0, 16, 0)
            };
            timeline = new TrackBar
            {
                Dock = DockStyle.Bottom, Minimum = 0, Maximum = 1000,
                TickStyle = TickStyle.None, Height = 34, Enabled = false,
                Orientation = Orientation.Horizontal
            };
            timeline.MouseDown += (s, e) => seeking = true;
            timeline.MouseUp += (s, e) => { Seek(); seeking = false; };
            timeline.KeyUp += (s, e) => Seek();
            Controls.Add(host);
            Controls.Add(timeline);
            Controls.Add(status);
            Controls.Add(controls);

            play.Click += (s, e) =>
            {
                if (playing) Pause();
                else
                {
                    if (ended) { player.Position = TimeSpan.Zero; ended = false; }
                    player.Play(); playing = true; play.Text = "Pause";
                }
            };
            stop.Click += (s, e) =>
            {
                player.Stop(); playing = false; ended = false; play.Text = "Play"; UpdateProgress();
            };
            player.MediaOpened += (s, e) =>
            {
                ready = true;
                play.Enabled = stop.Enabled = true;
                timeline.Enabled = player.NaturalDuration.HasTimeSpan;
                playing = true; play.Text = "Pause"; UpdateProgress();
            };
            player.MediaEnded += (s, e) => { Pause(); ended = true; UpdateProgress(); };
            player.MediaFailed += (s, e) =>
            {
                player.Close(); ready = playing = false;
                play.Enabled = stop.Enabled = timeline.Enabled = false;
                play.Text = "Play";
                status.Text = "Unable to play this video. Check that the MP4 file is valid and Windows media playback is available.";
            };
            timer = new Timer { Interval = 250 };
            timer.Tick += (s, e) => UpdateProgress();
            Shown += (s, e) =>
            {
                string path = Path.Combine(Application.StartupPath, "Videos", "InventorySystem.mp4");
                if (!File.Exists(path))
                {
                    status.Text = "The video is missing. Restore the Videos folder included with the application.";
                    return;
                }
                player.Source = new Uri(path, UriKind.Absolute);
                player.Play();
                timer.Start();
            };
            Deactivate += (s, e) => Pause();
            VisibleChanged += (s, e) => { if (!Visible) Pause(); };
        }

        private void Pause()
        {
            if (!ready || !playing) return;
            player.Pause(); playing = false; play.Text = "Play";
        }

        private void Seek()
        {
            if (!ready || !player.NaturalDuration.HasTimeSpan) return;
            player.Position = TimeSpan.FromSeconds(player.NaturalDuration.TimeSpan.TotalSeconds * timeline.Value / timeline.Maximum);
            ended = false;
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (!ready) return;
            var position = player.Position;
            if (!player.NaturalDuration.HasTimeSpan) { status.Text = position.ToString(@"hh\:mm\:ss"); return; }
            var duration = player.NaturalDuration.TimeSpan;
            if (!seeking && duration.TotalSeconds > 0)
                timeline.Value = Math.Max(0, Math.Min(timeline.Maximum, (int)(position.TotalSeconds / duration.TotalSeconds * timeline.Maximum)));
            status.Text = position.ToString(@"hh\:mm\:ss") + " / " + duration.ToString(@"hh\:mm\:ss");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Stop(); timer.Dispose(); player.Close();
            }
            base.Dispose(disposing);
        }
    }
}
