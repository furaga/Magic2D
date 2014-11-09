using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic2D
{
    /// <summary>
    /// セグメントの集合を入力として受け取り、骨格的に隣接しているものを一つのセグメントとして統合する
    /// たとえば、{seg1, seg2, seg3}のうち、seg1とseg3が隣接している場合 {seg2, seg4}（seg4 = seg1 + seg3）を出力する
    /// </summary>
    public class SegmentSynthesizer
    {
        public static List<Segment> Synthesize(List<Segment> segments)
        {
            return segments;
        }
    }
}
