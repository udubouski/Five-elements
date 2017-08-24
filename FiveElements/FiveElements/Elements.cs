using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Security.Cryptography;
using Android.Media;

namespace FiveElements
{
    [Activity(Label = "Elements")]
    public class Elements : Activity
    {
        const int stone = 1, scissors = 2, paper = 3, lizard = 4, spok = 5;
        const string win = "You win!", lose = "You lose!", draw = "Draw!";
        MediaPlayer arl,awin,alose,adraw;
        RNGCryptoServiceProvider CprytoRNG = new RNGCryptoServiceProvider();
        Hash myClass = new Hash();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Elements);

            int ch, count = 1,clk=0;
            string nameElem;
            String salt, hashedName;

            Vibrator vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Context.VibratorService);

            //manage components
            TextView hashView = FindViewById<TextView>(Resource.Id.hash);
            ImageView imgView = FindViewById<ImageView>(Resource.Id.imgView);
            TextView saltView = FindViewById<TextView>(Resource.Id.salt);
            Button right = FindViewById<Button>(Resource.Id.btnRight);
            Button left = FindViewById<Button>(Resource.Id.btnLeft);
            ImageButton elem = FindViewById<ImageButton>(Resource.Id.btnElem);
            TextView textSalt= FindViewById<TextView>(Resource.Id.txtSalt);
            TextView Salt = FindViewById<TextView>(Resource.Id.salt);
            TextView res = FindViewById<TextView>(Resource.Id.result);

            //audio components
            arl = MediaPlayer.Create(this, Resource.Raw.click1);
            awin = MediaPlayer.Create(this, Resource.Raw.win);
            alose = MediaPlayer.Create(this, Resource.Raw.lose);
            adraw = MediaPlayer.Create(this, Resource.Raw.draw2);

            load();
            //change icon
            right.Click += delegate {
                arl.Start();
                vibrator.Vibrate(50);
                count++;
                if (count >5) count = 1;
                view();
                rlClick();
            };

            left.Click += delegate {
                arl.Start();
                vibrator.Vibrate(50);
                count--;
                if (count < 1) count = 5;
                view();
                rlClick();
            };
            
            //button push
            elem.Click += delegate {
                clk++;
                if (clk == 2)
                {
                    rlClick();
                }
                else
                {
                    visibleComp();
                    saltView.Text = salt;
                    res.Text = selectElem(ch, count);
                    res.SetBackgroundColor(Android.Graphics.Color.Azure);
                    music(res.Text);
                }
            };

            //info hash
            imgView.Click += delegate {
                Dialog();
            };

            //visivility components
            void visibleComp()
            {
                imgView.Visibility = ViewStates.Visible;
                textSalt.Visibility = ViewStates.Visible;
                res.Visibility = ViewStates.Visible;
                Salt.Visibility = ViewStates.Visible;
            }

            //info_about
            void Dialog()
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Information");
                alert.SetMessage("hash("+Salt.Text + " + " + nameElem+") = " + hashView.Text);
                alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }

            //load data
            void load()
            {
                imgView.Visibility = ViewStates.Invisible;
                textSalt.Visibility = ViewStates.Invisible;
                ch = RandomIntFromRNG(1, 6);
                nameElem = funcElem(ch);
                salt = myClass.CreateSalt(10);
                hashedName = myClass.GenerateSHA256Hash(nameElem, salt);
                hashView.Text = hashedName;
            }

            //new game
            void rlClick()
            {
                load();
                res.Visibility = ViewStates.Invisible;
                Salt.Visibility = ViewStates.Invisible;
                clk = 0;
            }

            //load music
            void music(string text)
            {
                vibrator.Vibrate(300);
                if (text == win) awin.Start();
                else if (text == lose) alose.Start();
                else adraw.Start();

            }

            //change icon user
            void view()
            {
                if (count == stone) elem.SetImageResource(Resource.Drawable.stone);
                else if (count == scissors) elem.SetImageResource(Resource.Drawable.scissors);
                else if (count == paper) elem.SetImageResource(Resource.Drawable.paper);
                else if (count == lizard) elem.SetImageResource(Resource.Drawable.lizard);
                else if (count == spok) elem.SetImageResource(Resource.Drawable.spok);
            }

            //return text result
            string selectElem(int bot, int pep)
            {
                if (pep == stone && (bot == scissors || bot == lizard)) return win;
                if (pep == stone && (bot == paper || bot == spok)) return lose;
                if (pep == scissors && (bot == paper || bot == lizard)) return win;
                if (pep == scissors && (bot == stone || bot == spok)) return lose;
                if (pep == paper && (bot == stone || bot == spok)) return win;
                if (pep == paper && (bot == lizard || bot == scissors)) return lose;
                if (pep == lizard && (bot == spok || bot == paper)) return win;
                if (pep == lizard && (bot == scissors || bot == stone)) return lose;
                if (pep == spok && (bot == scissors || bot == stone)) return win;
                if (pep == spok && (bot == paper || bot == lizard)) return lose;
                else return draw;
            }

            //return string bot
            string funcElem(int p)
            {
                if (p == stone)
                {
                    imgView.SetImageResource(Resource.Drawable.stone);
                    return "stone";
                }
                else if (p == scissors)
                {
                    imgView.SetImageResource(Resource.Drawable.scissors);
                    return "scissors";
                }
                else if (p == paper)
                {
                    imgView.SetImageResource(Resource.Drawable.paper);
                    return "paper";
                }
                else if (p == lizard)
                {
                    imgView.SetImageResource(Resource.Drawable.lizard);
                    return "lizard";
                }
                else
                {
                    imgView.SetImageResource(Resource.Drawable.spok);
                    return "spok";
                }
            }

            // return a random integer between a min and max value.
            int RandomIntFromRNG(int min, int max)
            {
                byte[] four_bytes = new byte[4];
                CprytoRNG.GetBytes(four_bytes);
                UInt32 scale = BitConverter.ToUInt32(four_bytes, 0);
                return (int)(min + (max - min) * (scale / (uint.MaxValue + 1.0)));
            }

        }
    }
}