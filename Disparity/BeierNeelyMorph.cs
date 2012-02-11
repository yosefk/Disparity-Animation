﻿using AForge;
using AForge.Imaging;
using System;
using System.Drawing;

namespace Disparity
{
    class BeierNeelyMorph
    {
        public struct WeightParams
        {
            public float a;
            public float b;
            public float p;

            public float weight(float linelen, float distfromline)
            {
                double lp = Math.Pow(linelen, p);
                double w = lp / (a + distfromline);
                return (float)Math.Pow(w, b);
            }
        }

        static int dotp(IntPoint p1, IntPoint p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        static IntPoint perp(IntPoint p)
        {
            return new IntPoint(-p.Y, p.X);
        }
        
        public static Bitmap morph1(Bitmap srcimg, IntLine[] srclines, IntLine[] dstlines, WeightParams weightparams)
        {
            if (srclines.Length != dstlines.Length)
            {
                throw new ArgumentException(string.Format("srclines length ({0}) differs from dstlines length ({1})", srclines.Length, dstlines.Length));
            }
            int nl = srclines.Length;

            Bitmap dstimg = new Bitmap(srcimg.Width, srcimg.Height);
            int w = dstimg.Width;
            int h = dstimg.Height;
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    IntPoint X = new IntPoint(x,y); //"X" is the name from the paper
                    float DSUMX = 0;
                    float DSUMY = 0;
                    float weightsum = 0;
                    for (int i = 0; i < nl; ++i)
                    {
                        IntPoint P = dstlines[i].p;
                        IntPoint Q = dstlines[i].q;
                        IntPoint Ps = srclines[i].p;
                        IntPoint Qs = srclines[i].q;

                        //Q,P, X -> v,u
                        IntPoint QP = Q - P;
                        IntPoint XP = X - P;
                        float QP_norm = QP.EuclideanNorm();
                        float u = dotp(XP, QP) / (QP_norm*QP_norm);
                        float v = dotp(XP, perp(QP)) / QP_norm;

                        //v,u, Ps,Qs -> Xs
                        IntPoint QsPs = Qs - Ps;
                        IntPoint QsPs_perp = perp(QsPs);
                        float QsPs_norm = QsPs.EuclideanNorm();
                        float Xsi = Ps.X + u * QsPs.X + v * QsPs_perp.X / QsPs_norm;
                        float Ysi = Ps.Y + u * QsPs.Y + v * QsPs_perp.Y / QsPs_norm;

                        //Xs -> Di
                        float DiX = Xsi - x;
                        float DiY = Ysi - y;

                        //Q,P, X -> dist
                        float dist;
                        if (u <= 0)
                        {
                            dist = XP.EuclideanNorm();
                        }
                        else if (u >= 1)
                        {
                            dist = (X - Q).EuclideanNorm();
                        }
                        else
                        {
                            dist = Math.Abs(v);
                        }

                        //dist, Q,P -> weight
                        float weight = weightparams.weight(QP_norm, dist);

                        //Di, weight -> D update
                        DSUMX += DiX * weight;
                        DSUMY += DiY * weight;
                        weightsum += weight;
                    }
                    //DSUM, weightsum -> Xs
                    float Xs = x + DSUMX/weightsum;
                    float Ys = y + DSUMY/weightsum;
                    //FIXME: interpolate
                    if(Xs >=0 && Xs < w && Ys >= 0 && Ys < h) {
                        dstimg.SetPixel(x, y, srcimg.GetPixel((int)Xs, (int)Ys));
                    }
                }
            }
            return dstimg;
        }

        public static void test_morph1(String srcimg, String dstimg, String srcfeat, String dstfeat)
        {
            Bitmap srcbitmap = new Bitmap(Bitmap.FromFile(srcimg));
            IntLine[] srclines = Features.FromString(System.IO.File.ReadAllText(srcfeat)).barelines();
            IntLine[] dstlines = Features.FromString(System.IO.File.ReadAllText(dstfeat)).barelines();

            WeightParams weightparams = new WeightParams();
            weightparams.a = 0.1f;
            weightparams.b = 0.5f;
            weightparams.p = 0;
            Bitmap dstbitmap = morph1(srcbitmap, srclines, dstlines, weightparams);

            dstbitmap.Save(dstimg);
        }
    }
}