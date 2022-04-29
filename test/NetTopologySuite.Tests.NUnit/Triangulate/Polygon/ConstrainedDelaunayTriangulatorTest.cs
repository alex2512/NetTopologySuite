﻿using NetTopologySuite.Triangulate.Polygon;
using NUnit.Framework;

namespace NetTopologySuite.Tests.NUnit.Triangulate.Polygon
{
    public class ConstrainedDelaunayTriangulatorTest : GeometryTestCase
    {

        [Test]
        public void TestQuad()
        {
            CheckTri("POLYGON ((10 10, 20 40, 90 90, 90 10, 10 10))"
                , "GEOMETRYCOLLECTION (POLYGON ((10 10, 20 40, 90 10, 10 10)), POLYGON ((90 90, 20 40, 90 10, 90 90)))");
        }

        [Test]
        public void TestPent()
        {
            CheckTri("POLYGON ((10 10, 20 40, 90 90, 100 50, 90 10, 10 10))"
                , "GEOMETRYCOLLECTION (POLYGON ((10 10, 20 40, 90 10, 10 10)), POLYGON ((90 90, 20 40, 100 50, 90 90)), POLYGON ((100 50, 20 40, 90 10, 100 50)))");
        }

        [Test]
        public void TestHoleCW()
        {
            CheckTri("POLYGON ((10 90, 90 90, 90 20, 10 10, 10 90), (30 70, 80 70, 50 30, 30 70))"
                , "GEOMETRYCOLLECTION (POLYGON ((10 10, 10 90, 30 70, 10 10)), POLYGON ((10 10, 30 70, 50 30, 10 10)), POLYGON ((80 70, 30 70, 90 90, 80 70)), POLYGON ((10 90, 30 70, 90 90, 10 90)), POLYGON ((80 70, 90 90, 90 20, 80 70)), POLYGON ((90 20, 10 10, 50 30, 90 20)), POLYGON ((90 20, 50 30, 80 70, 90 20)))");
        }

        [Test]
        public void TestTouchingHoles()
        {
            CheckTri("POLYGON ((10 90, 90 90, 90 10, 10 10, 10 90), (20 80, 50 70, 30 30, 20 80), (70 20, 50 70, 80 80, 70 20))"
                , "GEOMETRYCOLLECTION (POLYGON ((10 10, 10 90, 20 80, 10 10)), POLYGON ((30 30, 50 70, 70 20, 30 30)), POLYGON ((80 80, 50 70, 20 80, 80 80)), POLYGON ((20 80, 10 90, 90 90, 20 80)), POLYGON ((10 10, 20 80, 30 30, 10 10)), POLYGON ((80 80, 20 80, 90 90, 80 80)), POLYGON ((70 20, 10 10, 30 30, 70 20)), POLYGON ((90 10, 80 80, 90 90, 90 10)), POLYGON ((10 10, 70 20, 90 10, 10 10)), POLYGON ((80 80, 90 10, 70 20, 80 80)))");
        }

        [Test]
        public void TestMultiPolygon()
        {
            CheckTri("MULTIPOLYGON (((10 10, 20 50, 50 50, 40 20, 10 10)), ((20 60, 60 60, 90 20, 90 90, 20 60)), ((10 90, 10 70, 40 70, 50 90, 10 90)))"
                , "GEOMETRYCOLLECTION (POLYGON ((10 10, 20 50, 40 20, 10 10)), POLYGON ((50 50, 20 50, 40 20, 50 50)), POLYGON ((90 90, 90 20, 60 60, 90 90)), POLYGON ((90 90, 60 60, 20 60, 90 90)), POLYGON ((10 70, 10 90, 40 70, 10 70)), POLYGON ((50 90, 10 90, 40 70, 50 90)))");
        }

        [Test]
        public void TestFail()
        {
            CheckTri(
          "POLYGON ((110 170, 138 272, 145 286, 152 296, 160 307, 303 307, 314 301, 332 287, 343 278, 352 270, 385 99, 374 89, 359 79, 178 89, 167 91, 153 99, 146 107, 173 157, 182 163, 191 170, 199 176, 208 184, 218 194, 226 203, 198 252, 188 247, 182 239, 175 231, 167 223, 161 213, 156 203, 155 198, 110 170))"
                );
        }

        private void CheckTri(string wkt, string wktExpected)
        {
            var geom = Read(wkt);
            var actual = ConstrainedDelaunayTriangulator.Triangulate(geom);
            var expected = Read(wktExpected);
            CheckEqual(expected, actual);
        }

        /**
         * Check union of result equals original geom
         * @param wkt
         */
        private void CheckTri(string wkt)
        {
            var geom = Read(wkt);
            var actual = ConstrainedDelaunayTriangulator.Triangulate(geom);
            var actualUnion = actual.Union();
            CheckEqual(geom, actualUnion);
        }
    }
}
