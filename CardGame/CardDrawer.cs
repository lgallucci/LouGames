using System;
using System.Collections.Generic;
using System.Text;
using SdlDotNet.Graphics;
using System.IO;
using System.Drawing;

namespace CardGame
{
    public class CardDrawer : IDisposable
    {
        private Surface cla, cl2, cl3, cl4, cl5, cl6, cl7, cl8, cl9, cl10, clj, clq, clk;
        private Surface hea, he2, he3, he4, he5, he6, he7, he8, he9, he10, hej, heq, hek;
        private Surface dia, di2, di3, di4, di5, di6, di7, di8, di9, di10, dij, diq, dik;
        private Surface spa, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, spj, spq, spk;

        private Surface Icla, Icl2, Icl3, Icl4, Icl5, Icl6, Icl7, Icl8, Icl9, Icl10, Iclj, Iclq, Iclk;
        private Surface Ihea, Ihe2, Ihe3, Ihe4, Ihe5, Ihe6, Ihe7, Ihe8, Ihe9, Ihe10, Ihej, Iheq, Ihek;
        private Surface Idia, Idi2, Idi3, Idi4, Idi5, Idi6, Idi7, Idi8, Idi9, Idi10, Idij, Idiq, Idik;
        private Surface Ispa, Isp2, Isp3, Isp4, Isp5, Isp6, Isp7, Isp8, Isp9, Isp10, Ispj, Ispq, Ispk;

        private Surface cardBack;

        /* Constructor */
        public CardDrawer()
        {
            LoadImages();
        }

        /* Load Images */
        private void LoadImages()
        {
            cla = new Surface(Properties.Resources.cla); cl2 = new Surface(Properties.Resources.cl2); cl3 = new Surface(Properties.Resources.cl3);
            cl4 = new Surface(Properties.Resources.cl4); cl5 = new Surface(Properties.Resources.cl5); cl6 = new Surface(Properties.Resources.cl6);
            cl7 = new Surface(Properties.Resources.cl7); cl8 = new Surface(Properties.Resources.cl8); cl9 = new Surface(Properties.Resources.cl9);
            cl10 = new Surface(Properties.Resources.cl10); clj = new Surface(Properties.Resources.clj); clq = new Surface(Properties.Resources.clq);
            clk = new Surface(Properties.Resources.clk);

            dia = new Surface(Properties.Resources.dia); di2 = new Surface(Properties.Resources.di2); di3 = new Surface(Properties.Resources.di3);
            di4 = new Surface(Properties.Resources.di4); di5 = new Surface(Properties.Resources.di5); di6 = new Surface(Properties.Resources.di6);
            di7 = new Surface(Properties.Resources.di7); di8 = new Surface(Properties.Resources.di8); di9 = new Surface(Properties.Resources.di9);
            di10 = new Surface(Properties.Resources.di10); dij = new Surface(Properties.Resources.dij); diq = new Surface(Properties.Resources.diq);
            dik = new Surface(Properties.Resources.dik);

            hea = new Surface(Properties.Resources.hea); he2 = new Surface(Properties.Resources.he2); he3 = new Surface(Properties.Resources.he3);
            he4 = new Surface(Properties.Resources.he4); he5 = new Surface(Properties.Resources.he5); he6 = new Surface(Properties.Resources.he6);
            he7 = new Surface(Properties.Resources.he7); he8 = new Surface(Properties.Resources.he8); he9 = new Surface(Properties.Resources.he9);
            he10 = new Surface(Properties.Resources.he10); hej = new Surface(Properties.Resources.hej); heq = new Surface(Properties.Resources.heq);
            hek = new Surface(Properties.Resources.hek);

            spa = new Surface(Properties.Resources.spa); sp2 = new Surface(Properties.Resources.sp2); sp3 = new Surface(Properties.Resources.sp3);
            sp4 = new Surface(Properties.Resources.sp4); sp5 = new Surface(Properties.Resources.sp5); sp6 = new Surface(Properties.Resources.sp6);
            sp7 = new Surface(Properties.Resources.sp7); sp8 = new Surface(Properties.Resources.sp8); sp9 = new Surface(Properties.Resources.sp9);
            sp10 = new Surface(Properties.Resources.sp10); spj = new Surface(Properties.Resources.spj); spq = new Surface(Properties.Resources.spq);
            spk = new Surface(Properties.Resources.spk);

            Icla = new Surface(Properties.Resources.Icla); Icl2 = new Surface(Properties.Resources.Icl2); Icl3 = new Surface(Properties.Resources.Icl3);
            Icl4 = new Surface(Properties.Resources.Icl4); Icl5 = new Surface(Properties.Resources.Icl5); Icl6 = new Surface(Properties.Resources.Icl6);
            Icl7 = new Surface(Properties.Resources.Icl7); Icl8 = new Surface(Properties.Resources.Icl8); Icl9 = new Surface(Properties.Resources.Icl9);
            Icl10 = new Surface(Properties.Resources.Icl10); Iclj = new Surface(Properties.Resources.Iclj); Iclq = new Surface(Properties.Resources.Iclq);
            Iclk = new Surface(Properties.Resources.Iclk);

            Idia = new Surface(Properties.Resources.Idia); Idi2 = new Surface(Properties.Resources.Idi2); Idi3 = new Surface(Properties.Resources.Idi3);
            Idi4 = new Surface(Properties.Resources.Idi4); Idi5 = new Surface(Properties.Resources.Idi5); Idi6 = new Surface(Properties.Resources.Idi6);
            Idi7 = new Surface(Properties.Resources.Idi7); Idi8 = new Surface(Properties.Resources.Idi8); Idi9 = new Surface(Properties.Resources.Idi9);
            Idi10 = new Surface(Properties.Resources.Idi10); Idij = new Surface(Properties.Resources.Idij); Idiq = new Surface(Properties.Resources.Idiq);
            Idik = new Surface(Properties.Resources.Idik);

            Ihea = new Surface(Properties.Resources.Ihea); Ihe2 = new Surface(Properties.Resources.Ihe2); Ihe3 = new Surface(Properties.Resources.Ihe3);
            Ihe4 = new Surface(Properties.Resources.Ihe4); Ihe5 = new Surface(Properties.Resources.Ihe5); Ihe6 = new Surface(Properties.Resources.Ihe6);
            Ihe7 = new Surface(Properties.Resources.Ihe7); Ihe8 = new Surface(Properties.Resources.Ihe8); Ihe9 = new Surface(Properties.Resources.Ihe9);
            Ihe10 = new Surface(Properties.Resources.Ihe10); Ihej = new Surface(Properties.Resources.Ihej); Iheq = new Surface(Properties.Resources.Iheq);
            Ihek = new Surface(Properties.Resources.Ihek);

            Ispa = new Surface(Properties.Resources.Ispa); Isp2 = new Surface(Properties.Resources.Isp2); Isp3 = new Surface(Properties.Resources.Isp3);
            Isp4 = new Surface(Properties.Resources.Isp4); Isp5 = new Surface(Properties.Resources.Isp5); Isp6 = new Surface(Properties.Resources.Isp6);
            Isp7 = new Surface(Properties.Resources.Isp7); Isp8 = new Surface(Properties.Resources.Isp8); Isp9 = new Surface(Properties.Resources.Isp9);
            Isp10 = new Surface(Properties.Resources.Isp10); Ispj = new Surface(Properties.Resources.Ispj); Ispq = new Surface(Properties.Resources.Ispq);
            Ispk = new Surface(Properties.Resources.Ispk);

            cardBack = new Surface(Properties.Resources.cardBack);
        }

        /* Get Images */
        private Surface GetCardInvertedImage(Card card)
        {
            switch (card.CardSuit)
            {
                case Suit.Clubs:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return Iclk;
                        case FaceValue.Queen:
                            return Iclq;
                        case FaceValue.Jack:
                            return Iclj;
                        case FaceValue.Ten:
                            return Icl10;
                        case FaceValue.Nine:
                            return Icl9;
                        case FaceValue.Eight:
                            return Icl8;
                        case FaceValue.Seven:
                            return Icl7;
                        case FaceValue.Six:
                            return Icl6;
                        case FaceValue.Five:
                            return Icl5;
                        case FaceValue.Four:
                            return Icl4;
                        case FaceValue.Three:
                            return Icl3;
                        case FaceValue.Two:
                            return Icl2;
                        case FaceValue.Ace:
                            return Icla;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Diamonds:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return Idik;
                        case FaceValue.Queen:
                            return Idiq;
                        case FaceValue.Jack:
                            return Idij;
                        case FaceValue.Ten:
                            return Idi10;
                        case FaceValue.Nine:
                            return Idi9;
                        case FaceValue.Eight:
                            return Idi8;
                        case FaceValue.Seven:
                            return Idi7;
                        case FaceValue.Six:
                            return Idi6;
                        case FaceValue.Five:
                            return Idi5;
                        case FaceValue.Four:
                            return Idi4;
                        case FaceValue.Three:
                            return Idi3;
                        case FaceValue.Two:
                            return Idi2;
                        case FaceValue.Ace:
                            return Idia;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Hearts:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return Ihek;
                        case FaceValue.Queen:
                            return Iheq;
                        case FaceValue.Jack:
                            return Ihej;
                        case FaceValue.Ten:
                            return Ihe10;
                        case FaceValue.Nine:
                            return Ihe9;
                        case FaceValue.Eight:
                            return Ihe8;
                        case FaceValue.Seven:
                            return Ihe7;
                        case FaceValue.Six:
                            return Ihe6;
                        case FaceValue.Five:
                            return Ihe5;
                        case FaceValue.Four:
                            return Ihe4;
                        case FaceValue.Three:
                            return Ihe3;
                        case FaceValue.Two:
                            return Ihe2;
                        case FaceValue.Ace:
                            return Ihea;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Spades:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return Ispk;
                        case FaceValue.Queen:
                            return Ispq;
                        case FaceValue.Jack:
                            return Ispj;
                        case FaceValue.Ten:
                            return Isp10;
                        case FaceValue.Nine:
                            return Isp9;
                        case FaceValue.Eight:
                            return Isp8;
                        case FaceValue.Seven:
                            return Isp7;
                        case FaceValue.Six:
                            return Isp6;
                        case FaceValue.Five:
                            return Isp5;
                        case FaceValue.Four:
                            return Isp4;
                        case FaceValue.Three:
                            return Isp3;
                        case FaceValue.Two:
                            return Isp2;
                        case FaceValue.Ace:
                            return Ispa;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                default:
                    throw new InvalidOperationException("Invalid Card!");
            }
        }

        private Surface GetCardImage(Card card)
        {
            switch (card.CardSuit)
            {
                case Suit.Clubs:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return clk;
                        case FaceValue.Queen:
                            return clq;
                        case FaceValue.Jack:
                            return clj;
                        case FaceValue.Ten:
                            return cl10;
                        case FaceValue.Nine:
                            return cl9;
                        case FaceValue.Eight:
                            return cl8;
                        case FaceValue.Seven:
                            return cl7;
                        case FaceValue.Six:
                            return cl6;
                        case FaceValue.Five:
                            return cl5;
                        case FaceValue.Four:
                            return cl4;
                        case FaceValue.Three:
                            return cl3;
                        case FaceValue.Two:
                            return cl2;
                        case FaceValue.Ace:
                            return cla;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Diamonds:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return dik;
                        case FaceValue.Queen:
                            return diq;
                        case FaceValue.Jack:
                            return dij;
                        case FaceValue.Ten:
                            return di10;
                        case FaceValue.Nine:
                            return di9;
                        case FaceValue.Eight:
                            return di8;
                        case FaceValue.Seven:
                            return di7;
                        case FaceValue.Six:
                            return di6;
                        case FaceValue.Five:
                            return di5;
                        case FaceValue.Four:
                            return di4;
                        case FaceValue.Three:
                            return di3;
                        case FaceValue.Two:
                            return di2;
                        case FaceValue.Ace:
                            return dia;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Hearts:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return hek;
                        case FaceValue.Queen:
                            return heq;
                        case FaceValue.Jack:
                            return hej;
                        case FaceValue.Ten:
                            return he10;
                        case FaceValue.Nine:
                            return he9;
                        case FaceValue.Eight:
                            return he8;
                        case FaceValue.Seven:
                            return he7;
                        case FaceValue.Six:
                            return he6;
                        case FaceValue.Five:
                            return he5;
                        case FaceValue.Four:
                            return he4;
                        case FaceValue.Three:
                            return he3;
                        case FaceValue.Two:
                            return he2;
                        case FaceValue.Ace:
                            return hea;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                case Suit.Spades:
                    switch (card.CardValue)
                    {
                        case FaceValue.King:
                            return spk;
                        case FaceValue.Queen:
                            return spq;
                        case FaceValue.Jack:
                            return spj;
                        case FaceValue.Ten:
                            return sp10;
                        case FaceValue.Nine:
                            return sp9;
                        case FaceValue.Eight:
                            return sp8;
                        case FaceValue.Seven:
                            return sp7;
                        case FaceValue.Six:
                            return sp6;
                        case FaceValue.Five:
                            return sp5;
                        case FaceValue.Four:
                            return sp4;
                        case FaceValue.Three:
                            return sp3;
                        case FaceValue.Two:
                            return sp2;
                        case FaceValue.Ace:
                            return spa;
                        default:
                            throw new InvalidOperationException("Invalid Card!");
                    }
                default:
                    throw new InvalidOperationException("Invalid Card!");
            }
        }

        /* Drawing Methods */
        public Surface DrawCardFaceUp(Card card)
        {
            Surface ret = GetCardImage(card);
            ret.Transparent = true;
            ret.TransparentColor = Color.FromArgb(255, 000, 255);
            return ret;
        }

        public Surface DrawCardFacedown()
        {
            Surface ret = cardBack;
            ret.Transparent = true;
            ret.TransparentColor = Color.FromArgb(255, 000, 255);
            return ret;
        }

        public Surface DrawCardInverted(Card card)
        {
            Surface ret = GetCardInvertedImage(card);
            ret.Transparent = true;
            ret.TransparentColor = Color.FromArgb(000, 255, 000);
            return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            cla.Dispose(); cl2.Dispose(); cl3.Dispose(); cl4.Dispose(); cl5.Dispose(); cl6.Dispose(); cl7.Dispose();
            cl8.Dispose(); cl9.Dispose(); cl10.Dispose(); clj.Dispose(); clq.Dispose(); clk.Dispose();
            hea.Dispose(); he2.Dispose(); he3.Dispose(); he4.Dispose(); he5.Dispose(); he6.Dispose(); he7.Dispose();
            he8.Dispose(); he9.Dispose(); he10.Dispose(); hej.Dispose(); heq.Dispose(); hek.Dispose();
            dia.Dispose(); di2.Dispose(); di3.Dispose(); di4.Dispose(); di5.Dispose(); di6.Dispose(); di7.Dispose();
            di8.Dispose(); di9.Dispose(); di10.Dispose(); dij.Dispose(); diq.Dispose(); dik.Dispose();
            spa.Dispose(); sp2.Dispose(); sp3.Dispose(); sp4.Dispose(); sp5.Dispose(); sp6.Dispose(); sp7.Dispose();
            sp8.Dispose(); sp9.Dispose(); sp10.Dispose(); spj.Dispose(); spq.Dispose(); spk.Dispose();

            Icla.Dispose(); Icl2.Dispose(); Icl3.Dispose(); Icl4.Dispose(); Icl5.Dispose(); Icl6.Dispose();
            Icl7.Dispose(); Icl8.Dispose(); Icl9.Dispose(); Icl10.Dispose(); Iclj.Dispose(); Iclq.Dispose(); Iclk.Dispose();
            Ihea.Dispose(); Ihe2.Dispose(); Ihe3.Dispose(); Ihe4.Dispose(); Ihe5.Dispose(); Ihe6.Dispose();
            Ihe7.Dispose(); Ihe8.Dispose(); Ihe9.Dispose(); Ihe10.Dispose(); Ihej.Dispose(); Iheq.Dispose(); Ihek.Dispose();
            Idia.Dispose(); Idi2.Dispose(); Idi3.Dispose(); Idi4.Dispose(); Idi5.Dispose(); Idi6.Dispose();
            Idi7.Dispose(); Idi8.Dispose(); Idi9.Dispose(); Idi10.Dispose(); Idij.Dispose(); Idiq.Dispose(); Idik.Dispose();
            Ispa.Dispose(); Isp2.Dispose(); Isp3.Dispose(); Isp4.Dispose(); Isp5.Dispose(); Isp6.Dispose();
            Isp7.Dispose(); Isp8.Dispose(); Isp9.Dispose(); Isp10.Dispose(); Ispj.Dispose(); Ispq.Dispose(); Ispk.Dispose();

            cardBack.Dispose();
        }
        #endregion
    }
}
