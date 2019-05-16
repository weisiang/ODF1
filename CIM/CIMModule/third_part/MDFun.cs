﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MxLib
{
    class MDFun
    {

        [DllImport("MDFUNC32.dll", EntryPoint = "mdopen")]
        static extern short mdOpen(short Chan, short Mode, ref int Path);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdclose")]
        static extern short mdClose(int Path);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdsend")]
        static extern short mdSend(int Path, short Stno, short Devtyp, short devno, ref short size, ref short buf);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdreceive")]
        static extern short mdReceive(int Path, short Stno, short Devtyp, short devno, ref short size, ref short buf);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mddevset")]
        static extern short mdDevSet(int Path, short Stno, short Devtyp, short devno);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mddevrst")]
        static extern short mdDevRst(int Path, short Stno, short Devtyp, short devno);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrandw")]
        static extern short mdRandW(int Path, short Stno, ref short dev, ref short buf, short bufsiz);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrandr")]
        static extern short mdRandR(int Path, short Stno, ref short dev, ref short buf, short bufsiz);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdcontrol")]
        static extern short mdControl(int Path, short Stno, short buf);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdtyperead")]
        static extern short mdTypeRead(int Path, short Stno, ref short buf);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdLedRead(int Path, ref short buf);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdModRead(int Path, ref short Mode);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdModSet(int Path, short Mode);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdRst(int Path);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdSwRead(int Path, ref short buf);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdBdVerRead(int Path, ref short buf);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdInit(int Path);

        [DllImport("MDFUNC32.DLL")]
        static extern short mdWaitBdEvent(int Path, ref short eventno, int timeout, ref short signaledno, ref short details);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdsendex")]
        static extern int mdSendEx(int Path, int Netno, int Stno, int Devtyp, int devno, ref int size, ref short buf);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdreceiveex")]
        static extern int mdReceiveEx(int Path, int Netno, int Stno, int Devtyp, int devno, ref int size, ref short buf);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mddevsetex")]
        static extern int mdDevSetEx(int Path, int Netno, int Stno, int Devtyp, int devno);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mddevrstex")]
        static extern int mdDevRstEx(int Path, int Netno, int Stno, int Devtyp, int devno);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrandwex")]
        static extern int mdRandWEx(int Path, int Netno, int Stno, ref int dev, ref short buf, int bufsiz);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrandrex")]
        static extern int mdRandREx(int Path, int Netno, int Stno, ref int dev, ref short buf, int bufsiz);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrembufwriteex")]
        static extern int mdRemBufWriteEx(int Path, int Netno, int Stno, int Offset, ref int size, ref short data);

        [DllImport("MDFUNC32.DLL", EntryPoint = "mdrembufreadex")]
        static extern int mdRemBufReadEx(int Path, int Netno, int Stno, int Offset, ref int size, ref short data);

        public const short DevX = 1;

        public const short DevLX1 = 1001;

        public const short DevLX2 = 1002;

        public const short DevLX3 = 1003;

        public const short DevLX4 = 1004;

        public const short DevLX5 = 1005;

        public const short DevLX6 = 1006;

        public const short DevLX7 = 1007;

        public const short DevLX8 = 1008;

        public const short DevLX9 = 1009;

        public const short DevLX10 = 1010;

        public const short DevLX11 = 1011;

        public const short DevLX12 = 1012;

        public const short DevLX13 = 1013;

        public const short DevLX14 = 1014;

        public const short DevLX15 = 1015;

        public const short DevLX16 = 1016;

        public const short DevLX17 = 1017;

        public const short DevLX18 = 1018;

        public const short DevLX19 = 1019;

        public const short DevLX20 = 1020;

        public const short DevLX21 = 1021;

        public const short DevLX22 = 1022;

        public const short DevLX23 = 1023;

        public const short DevLX24 = 1024;

        public const short DevLX25 = 1025;

        public const short DevLX26 = 1026;

        public const short DevLX27 = 1027;

        public const short DevLX28 = 1028;

        public const short DevLX29 = 1029;

        public const short DevLX30 = 1030;

        public const short DevLX31 = 1031;

        public const short DevLX32 = 1032;

        public const short DevLX33 = 1033;

        public const short DevLX34 = 1034;

        public const short DevLX35 = 1035;

        public const short DevLX36 = 1036;

        public const short DevLX37 = 1037;

        public const short DevLX38 = 1038;

        public const short DevLX39 = 1039;

        public const short DevLX40 = 1040;

        public const short DevLX41 = 1041;

        public const short DevLX42 = 1042;

        public const short DevLX43 = 1043;

        public const short DevLX44 = 1044;

        public const short DevLX45 = 1045;

        public const short DevLX46 = 1046;

        public const short DevLX47 = 1047;

        public const short DevLX48 = 1048;

        public const short DevLX49 = 1049;

        public const short DevLX50 = 1050;

        public const short DevLX51 = 1051;

        public const short DevLX52 = 1052;

        public const short DevLX53 = 1053;

        public const short DevLX54 = 1054;

        public const short DevLX55 = 1055;

        public const short DevLX56 = 1056;

        public const short DevLX57 = 1057;

        public const short DevLX58 = 1058;

        public const short DevLX59 = 1059;

        public const short DevLX60 = 1060;

        public const short DevLX61 = 1061;

        public const short DevLX62 = 1062;

        public const short DevLX63 = 1063;

        public const short DevLX64 = 1064;

        public const short DevLX65 = 1065;

        public const short DevLX66 = 1066;

        public const short DevLX67 = 1067;

        public const short DevLX68 = 1068;

        public const short DevLX69 = 1069;

        public const short DevLX70 = 1070;

        public const short DevLX71 = 1071;

        public const short DevLX72 = 1072;

        public const short DevLX73 = 1073;

        public const short DevLX74 = 1074;

        public const short DevLX75 = 1075;

        public const short DevLX76 = 1076;

        public const short DevLX77 = 1077;

        public const short DevLX78 = 1078;

        public const short DevLX79 = 1079;

        public const short DevLX80 = 1080;

        public const short DevLX81 = 1081;

        public const short DevLX82 = 1082;

        public const short DevLX83 = 1083;

        public const short DevLX84 = 1084;

        public const short DevLX85 = 1085;

        public const short DevLX86 = 1086;

        public const short DevLX87 = 1087;

        public const short DevLX88 = 1088;

        public const short DevLX89 = 1089;

        public const short DevLX90 = 1090;

        public const short DevLX91 = 1091;

        public const short DevLX92 = 1092;

        public const short DevLX93 = 1093;

        public const short DevLX94 = 1094;

        public const short DevLX95 = 1095;

        public const short DevLX96 = 1096;

        public const short DevLX97 = 1097;

        public const short DevLX98 = 1098;

        public const short DevLX99 = 1099;

        public const short DevLX100 = 1100;

        public const short DevLX101 = 1101;

        public const short DevLX102 = 1102;

        public const short DevLX103 = 1103;

        public const short DevLX104 = 1104;

        public const short DevLX105 = 1105;

        public const short DevLX106 = 1106;

        public const short DevLX107 = 1107;

        public const short DevLX108 = 1108;

        public const short DevLX109 = 1109;

        public const short DevLX110 = 1110;

        public const short DevLX111 = 1111;

        public const short DevLX112 = 1112;

        public const short DevLX113 = 1113;

        public const short DevLX114 = 1114;

        public const short DevLX115 = 1115;

        public const short DevLX116 = 1116;

        public const short DevLX117 = 1117;

        public const short DevLX118 = 1118;

        public const short DevLX119 = 1119;

        public const short DevLX120 = 1120;

        public const short DevLX121 = 1121;

        public const short DevLX122 = 1122;

        public const short DevLX123 = 1123;

        public const short DevLX124 = 1124;

        public const short DevLX125 = 1125;

        public const short DevLX126 = 1126;

        public const short DevLX127 = 1127;

        public const short DevLX128 = 1128;

        public const short DevLX129 = 1129;

        public const short DevLX130 = 1130;

        public const short DevLX131 = 1131;

        public const short DevLX132 = 1132;

        public const short DevLX133 = 1133;

        public const short DevLX134 = 1134;

        public const short DevLX135 = 1135;

        public const short DevLX136 = 1136;

        public const short DevLX137 = 1137;

        public const short DevLX138 = 1138;

        public const short DevLX139 = 1139;

        public const short DevLX140 = 1140;

        public const short DevLX141 = 1141;

        public const short DevLX142 = 1142;

        public const short DevLX143 = 1143;

        public const short DevLX144 = 1144;

        public const short DevLX145 = 1145;

        public const short DevLX146 = 1146;

        public const short DevLX147 = 1147;

        public const short DevLX148 = 1148;

        public const short DevLX149 = 1149;

        public const short DevLX150 = 1150;

        public const short DevLX151 = 1151;

        public const short DevLX152 = 1152;

        public const short DevLX153 = 1153;

        public const short DevLX154 = 1154;

        public const short DevLX155 = 1155;

        public const short DevLX156 = 1156;

        public const short DevLX157 = 1157;

        public const short DevLX158 = 1158;

        public const short DevLX159 = 1159;

        public const short DevLX160 = 1160;

        public const short DevLX161 = 1161;

        public const short DevLX162 = 1162;

        public const short DevLX163 = 1163;

        public const short DevLX164 = 1164;

        public const short DevLX165 = 1165;

        public const short DevLX166 = 1166;

        public const short DevLX167 = 1167;

        public const short DevLX168 = 1168;

        public const short DevLX169 = 1169;

        public const short DevLX170 = 1170;

        public const short DevLX171 = 1171;

        public const short DevLX172 = 1172;

        public const short DevLX173 = 1173;

        public const short DevLX174 = 1174;

        public const short DevLX175 = 1175;

        public const short DevLX176 = 1176;

        public const short DevLX177 = 1177;

        public const short DevLX178 = 1178;

        public const short DevLX179 = 1179;

        public const short DevLX180 = 1180;

        public const short DevLX181 = 1181;

        public const short DevLX182 = 1182;

        public const short DevLX183 = 1183;

        public const short DevLX184 = 1184;

        public const short DevLX185 = 1185;

        public const short DevLX186 = 1186;

        public const short DevLX187 = 1187;

        public const short DevLX188 = 1188;

        public const short DevLX189 = 1189;

        public const short DevLX190 = 1190;

        public const short DevLX191 = 1191;

        public const short DevLX192 = 1192;

        public const short DevLX193 = 1193;

        public const short DevLX194 = 1194;

        public const short DevLX195 = 1195;

        public const short DevLX196 = 1196;

        public const short DevLX197 = 1197;

        public const short DevLX198 = 1198;

        public const short DevLX199 = 1199;

        public const short DevLX200 = 1200;

        public const short DevLX201 = 1201;

        public const short DevLX202 = 1202;

        public const short DevLX203 = 1203;

        public const short DevLX204 = 1204;

        public const short DevLX205 = 1205;

        public const short DevLX206 = 1206;

        public const short DevLX207 = 1207;

        public const short DevLX208 = 1208;

        public const short DevLX209 = 1209;

        public const short DevLX210 = 1210;

        public const short DevLX211 = 1211;

        public const short DevLX212 = 1212;

        public const short DevLX213 = 1213;

        public const short DevLX214 = 1214;

        public const short DevLX215 = 1215;

        public const short DevLX216 = 1216;

        public const short DevLX217 = 1217;

        public const short DevLX218 = 1218;

        public const short DevLX219 = 1219;

        public const short DevLX220 = 1220;

        public const short DevLX221 = 1221;

        public const short DevLX222 = 1222;

        public const short DevLX223 = 1223;

        public const short DevLX224 = 1224;

        public const short DevLX225 = 1225;

        public const short DevLX226 = 1226;

        public const short DevLX227 = 1227;

        public const short DevLX228 = 1228;

        public const short DevLX229 = 1229;

        public const short DevLX230 = 1230;

        public const short DevLX231 = 1231;

        public const short DevLX232 = 1232;

        public const short DevLX233 = 1233;

        public const short DevLX234 = 1234;

        public const short DevLX235 = 1235;

        public const short DevLX236 = 1236;

        public const short DevLX237 = 1237;

        public const short DevLX238 = 1238;

        public const short DevLX239 = 1239;

        public const short DevLX240 = 1240;

        public const short DevLX241 = 1241;

        public const short DevLX242 = 1242;

        public const short DevLX243 = 1243;

        public const short DevLX244 = 1244;

        public const short DevLX245 = 1245;

        public const short DevLX246 = 1246;

        public const short DevLX247 = 1247;

        public const short DevLX248 = 1248;

        public const short DevLX249 = 1249;

        public const short DevLX250 = 1250;

        public const short DevLX251 = 1251;

        public const short DevLX252 = 1252;

        public const short DevLX253 = 1253;

        public const short DevLX254 = 1254;

        public const short DevLX255 = 1255;

        public const short DevY = 2;

        public const short DevLY1 = 2001;

        public const short DevLY2 = 2002;

        public const short DevLY3 = 2003;

        public const short DevLY4 = 2004;

        public const short DevLY5 = 2005;

        public const short DevLY6 = 2006;

        public const short DevLY7 = 2007;

        public const short DevLY8 = 2008;

        public const short DevLY9 = 2009;

        public const short DevLY10 = 2010;

        public const short DevLY11 = 2011;

        public const short DevLY12 = 2012;

        public const short DevLY13 = 2013;

        public const short DevLY14 = 2014;

        public const short DevLY15 = 2015;

        public const short DevLY16 = 2016;

        public const short DevLY17 = 2017;

        public const short DevLY18 = 2018;

        public const short DevLY19 = 2019;

        public const short DevLY20 = 2020;

        public const short DevLY21 = 2021;

        public const short DevLY22 = 2022;

        public const short DevLY23 = 2023;

        public const short DevLY24 = 2024;

        public const short DevLY25 = 2025;

        public const short DevLY26 = 2026;

        public const short DevLY27 = 2027;

        public const short DevLY28 = 2028;

        public const short DevLY29 = 2029;

        public const short DevLY30 = 2030;

        public const short DevLY31 = 2031;

        public const short DevLY32 = 2032;

        public const short DevLY33 = 2033;

        public const short DevLY34 = 2034;

        public const short DevLY35 = 2035;

        public const short DevLY36 = 2036;

        public const short DevLY37 = 2037;

        public const short DevLY38 = 2038;

        public const short DevLY39 = 2039;

        public const short DevLY40 = 2040;

        public const short DevLY41 = 2041;

        public const short DevLY42 = 2042;

        public const short DevLY43 = 2043;

        public const short DevLY44 = 2044;

        public const short DevLY45 = 2045;

        public const short DevLY46 = 2046;

        public const short DevLY47 = 2047;

        public const short DevLY48 = 2048;

        public const short DevLY49 = 2049;

        public const short DevLY50 = 2050;

        public const short DevLY51 = 2051;

        public const short DevLY52 = 2052;

        public const short DevLY53 = 2053;

        public const short DevLY54 = 2054;

        public const short DevLY55 = 2055;

        public const short DevLY56 = 2056;

        public const short DevLY57 = 2057;

        public const short DevLY58 = 2058;

        public const short DevLY59 = 2059;

        public const short DevLY60 = 2060;

        public const short DevLY61 = 2061;

        public const short DevLY62 = 2062;

        public const short DevLY63 = 2063;

        public const short DevLY64 = 2064;

        public const short DevLY65 = 2065;

        public const short DevLY66 = 2066;

        public const short DevLY67 = 2067;

        public const short DevLY68 = 2068;

        public const short DevLY69 = 2069;

        public const short DevLY70 = 2070;

        public const short DevLY71 = 2071;

        public const short DevLY72 = 2072;

        public const short DevLY73 = 2073;

        public const short DevLY74 = 2074;

        public const short DevLY75 = 2075;

        public const short DevLY76 = 2076;

        public const short DevLY77 = 2077;

        public const short DevLY78 = 2078;

        public const short DevLY79 = 2079;

        public const short DevLY80 = 2080;

        public const short DevLY81 = 2081;

        public const short DevLY82 = 2082;

        public const short DevLY83 = 2083;

        public const short DevLY84 = 2084;

        public const short DevLY85 = 2085;

        public const short DevLY86 = 2086;

        public const short DevLY87 = 2087;

        public const short DevLY88 = 2088;

        public const short DevLY89 = 2089;

        public const short DevLY90 = 2090;

        public const short DevLY91 = 2091;

        public const short DevLY92 = 2092;

        public const short DevLY93 = 2093;

        public const short DevLY94 = 2094;

        public const short DevLY95 = 2095;

        public const short DevLY96 = 2096;

        public const short DevLY97 = 2097;

        public const short DevLY98 = 2098;

        public const short DevLY99 = 2099;

        public const short DevLY100 = 2100;

        public const short DevLY101 = 2101;

        public const short DevLY102 = 2102;

        public const short DevLY103 = 2103;

        public const short DevLY104 = 2104;

        public const short DevLY105 = 2105;

        public const short DevLY106 = 2106;

        public const short DevLY107 = 2107;

        public const short DevLY108 = 2108;

        public const short DevLY109 = 2109;

        public const short DevLY110 = 2110;

        public const short DevLY111 = 2111;

        public const short DevLY112 = 2112;

        public const short DevLY113 = 2113;

        public const short DevLY114 = 2114;

        public const short DevLY115 = 2115;

        public const short DevLY116 = 2116;

        public const short DevLY117 = 2117;

        public const short DevLY118 = 2118;

        public const short DevLY119 = 2119;

        public const short DevLY120 = 2120;

        public const short DevLY121 = 2121;

        public const short DevLY122 = 2122;

        public const short DevLY123 = 2123;

        public const short DevLY124 = 2124;

        public const short DevLY125 = 2125;

        public const short DevLY126 = 2126;

        public const short DevLY127 = 2127;

        public const short DevLY128 = 2128;

        public const short DevLY129 = 2129;

        public const short DevLY130 = 2130;

        public const short DevLY131 = 2131;

        public const short DevLY132 = 2132;

        public const short DevLY133 = 2133;

        public const short DevLY134 = 2134;

        public const short DevLY135 = 2135;

        public const short DevLY136 = 2136;

        public const short DevLY137 = 2137;

        public const short DevLY138 = 2138;

        public const short DevLY139 = 2139;

        public const short DevLY140 = 2140;

        public const short DevLY141 = 2141;

        public const short DevLY142 = 2142;

        public const short DevLY143 = 2143;

        public const short DevLY144 = 2144;

        public const short DevLY145 = 2145;

        public const short DevLY146 = 2146;

        public const short DevLY147 = 2147;

        public const short DevLY148 = 2148;

        public const short DevLY149 = 2149;

        public const short DevLY150 = 2150;

        public const short DevLY151 = 2151;

        public const short DevLY152 = 2152;

        public const short DevLY153 = 2153;

        public const short DevLY154 = 2154;

        public const short DevLY155 = 2155;

        public const short DevLY156 = 2156;

        public const short DevLY157 = 2157;

        public const short DevLY158 = 2158;

        public const short DevLY159 = 2159;

        public const short DevLY160 = 2160;

        public const short DevLY161 = 2161;

        public const short DevLY162 = 2162;

        public const short DevLY163 = 2163;

        public const short DevLY164 = 2164;

        public const short DevLY165 = 2165;

        public const short DevLY166 = 2166;

        public const short DevLY167 = 2167;

        public const short DevLY168 = 2168;

        public const short DevLY169 = 2169;

        public const short DevLY170 = 2170;

        public const short DevLY171 = 2171;

        public const short DevLY172 = 2172;

        public const short DevLY173 = 2173;

        public const short DevLY174 = 2174;

        public const short DevLY175 = 2175;

        public const short DevLY176 = 2176;

        public const short DevLY177 = 2177;

        public const short DevLY178 = 2178;

        public const short DevLY179 = 2179;

        public const short DevLY180 = 2180;

        public const short DevLY181 = 2181;

        public const short DevLY182 = 2182;

        public const short DevLY183 = 2183;

        public const short DevLY184 = 2184;

        public const short DevLY185 = 2185;

        public const short DevLY186 = 2186;

        public const short DevLY187 = 2187;

        public const short DevLY188 = 2188;

        public const short DevLY189 = 2189;

        public const short DevLY190 = 2190;

        public const short DevLY191 = 2191;

        public const short DevLY192 = 2192;

        public const short DevLY193 = 2193;

        public const short DevLY194 = 2194;

        public const short DevLY195 = 2195;

        public const short DevLY196 = 2196;

        public const short DevLY197 = 2197;

        public const short DevLY198 = 2198;

        public const short DevLY199 = 2199;

        public const short DevLY200 = 2200;

        public const short DevLY201 = 2201;

        public const short DevLY202 = 2202;

        public const short DevLY203 = 2203;

        public const short DevLY204 = 2204;

        public const short DevLY205 = 2205;

        public const short DevLY206 = 2206;

        public const short DevLY207 = 2207;

        public const short DevLY208 = 2208;

        public const short DevLY209 = 2209;

        public const short DevLY210 = 2210;

        public const short DevLY211 = 2211;

        public const short DevLY212 = 2212;

        public const short DevLY213 = 2213;

        public const short DevLY214 = 2214;

        public const short DevLY215 = 2215;

        public const short DevLY216 = 2216;

        public const short DevLY217 = 2217;

        public const short DevLY218 = 2218;

        public const short DevLY219 = 2219;

        public const short DevLY220 = 2220;

        public const short DevLY221 = 2221;

        public const short DevLY222 = 2222;

        public const short DevLY223 = 2223;

        public const short DevLY224 = 2224;

        public const short DevLY225 = 2225;

        public const short DevLY226 = 2226;

        public const short DevLY227 = 2227;

        public const short DevLY228 = 2228;

        public const short DevLY229 = 2229;

        public const short DevLY230 = 2230;

        public const short DevLY231 = 2231;

        public const short DevLY232 = 2232;

        public const short DevLY233 = 2233;

        public const short DevLY234 = 2234;

        public const short DevLY235 = 2235;

        public const short DevLY236 = 2236;

        public const short DevLY237 = 2237;

        public const short DevLY238 = 2238;

        public const short DevLY239 = 2239;

        public const short DevLY240 = 2240;

        public const short DevLY241 = 2241;

        public const short DevLY242 = 2242;

        public const short DevLY243 = 2243;

        public const short DevLY244 = 2244;

        public const short DevLY245 = 2245;

        public const short DevLY246 = 2246;

        public const short DevLY247 = 2247;

        public const short DevLY248 = 2248;

        public const short DevLY249 = 2249;

        public const short DevLY250 = 2250;

        public const short DevLY251 = 2251;

        public const short DevLY252 = 2252;

        public const short DevLY253 = 2253;

        public const short DevLY254 = 2254;

        public const short DevLY255 = 2255;

        public const short DevL = 3;

        public const short DevM = 4;

        public const short DevSM = 5;

        public const short DevF = 6;

        public const short DevTT = 7;

        public const short DevTC = 8;

        public const short DevCT = 9;

        public const short DevCC = 10;

        public const short DevTN = 11;

        public const short DevCN = 12;

        public const short DevD = 13;

        public const short DevSD = 14;

        public const short DevTM = 15;

        public const short DevTS = 16;

        public const short DevTS2 = 16002;

        public const short DevTS3 = 16003;

        public const short DevCM = 17;

        public const short DevCS = 18;

        public const short DevCS2 = 18002;

        public const short DevCS3 = 18003;

        public const short DevA = 19;

        public const short DevZ = 20;

        public const short DevV = 21;

        public const short DevR = 22;

        public const short DevZR = 220;

        public const short DevB = 23;

        public const short DevLB1 = 23001;

        public const short DevLB2 = 23002;

        public const short DevLB3 = 23003;

        public const short DevLB4 = 23004;

        public const short DevLB5 = 23005;

        public const short DevLB6 = 23006;

        public const short DevLB7 = 23007;

        public const short DevLB8 = 23008;

        public const short DevLB9 = 23009;

        public const short DevLB10 = 23010;

        public const short DevLB11 = 23011;

        public const short DevLB12 = 23012;

        public const short DevLB13 = 23013;

        public const short DevLB14 = 23014;

        public const short DevLB15 = 23015;

        public const short DevLB16 = 23016;

        public const short DevLB17 = 23017;

        public const short DevLB18 = 23018;

        public const short DevLB19 = 23019;

        public const short DevLB20 = 23020;

        public const short DevLB21 = 23021;

        public const short DevLB22 = 23022;

        public const short DevLB23 = 23023;

        public const short DevLB24 = 23024;

        public const short DevLB25 = 23025;

        public const short DevLB26 = 23026;

        public const short DevLB27 = 23027;

        public const short DevLB28 = 23028;

        public const short DevLB29 = 23029;

        public const short DevLB30 = 23030;

        public const short DevLB31 = 23031;

        public const short DevLB32 = 23032;

        public const short DevLB33 = 23033;

        public const short DevLB34 = 23034;

        public const short DevLB35 = 23035;

        public const short DevLB36 = 23036;

        public const short DevLB37 = 23037;

        public const short DevLB38 = 23038;

        public const short DevLB39 = 23039;

        public const short DevLB40 = 23040;

        public const short DevLB41 = 23041;

        public const short DevLB42 = 23042;

        public const short DevLB43 = 23043;

        public const short DevLB44 = 23044;

        public const short DevLB45 = 23045;

        public const short DevLB46 = 23046;

        public const short DevLB47 = 23047;

        public const short DevLB48 = 23048;

        public const short DevLB49 = 23049;

        public const short DevLB50 = 23050;

        public const short DevLB51 = 23051;

        public const short DevLB52 = 23052;

        public const short DevLB53 = 23053;

        public const short DevLB54 = 23054;

        public const short DevLB55 = 23055;

        public const short DevLB56 = 23056;

        public const short DevLB57 = 23057;

        public const short DevLB58 = 23058;

        public const short DevLB59 = 23059;

        public const short DevLB60 = 23060;

        public const short DevLB61 = 23061;

        public const short DevLB62 = 23062;

        public const short DevLB63 = 23063;

        public const short DevLB64 = 23064;

        public const short DevLB65 = 23065;

        public const short DevLB66 = 23066;

        public const short DevLB67 = 23067;

        public const short DevLB68 = 23068;

        public const short DevLB69 = 23069;

        public const short DevLB70 = 23070;

        public const short DevLB71 = 23071;

        public const short DevLB72 = 23072;

        public const short DevLB73 = 23073;

        public const short DevLB74 = 23074;

        public const short DevLB75 = 23075;

        public const short DevLB76 = 23076;

        public const short DevLB77 = 23077;

        public const short DevLB78 = 23078;

        public const short DevLB79 = 23079;

        public const short DevLB80 = 23080;

        public const short DevLB81 = 23081;

        public const short DevLB82 = 23082;

        public const short DevLB83 = 23083;

        public const short DevLB84 = 23084;

        public const short DevLB85 = 23085;

        public const short DevLB86 = 23086;

        public const short DevLB87 = 23087;

        public const short DevLB88 = 23088;

        public const short DevLB89 = 23089;

        public const short DevLB90 = 23090;

        public const short DevLB91 = 23091;

        public const short DevLB92 = 23092;

        public const short DevLB93 = 23093;

        public const short DevLB94 = 23094;

        public const short DevLB95 = 23095;

        public const short DevLB96 = 23096;

        public const short DevLB97 = 23097;

        public const short DevLB98 = 23098;

        public const short DevLB99 = 23099;

        public const short DevLB100 = 23100;

        public const short DevLB101 = 23101;

        public const short DevLB102 = 23102;

        public const short DevLB103 = 23103;

        public const short DevLB104 = 23104;

        public const short DevLB105 = 23105;

        public const short DevLB106 = 23106;

        public const short DevLB107 = 23107;

        public const short DevLB108 = 23108;

        public const short DevLB109 = 23109;

        public const short DevLB110 = 23110;

        public const short DevLB111 = 23111;

        public const short DevLB112 = 23112;

        public const short DevLB113 = 23113;

        public const short DevLB114 = 23114;

        public const short DevLB115 = 23115;

        public const short DevLB116 = 23116;

        public const short DevLB117 = 23117;

        public const short DevLB118 = 23118;

        public const short DevLB119 = 23119;

        public const short DevLB120 = 23120;

        public const short DevLB121 = 23121;

        public const short DevLB122 = 23122;

        public const short DevLB123 = 23123;

        public const short DevLB124 = 23124;

        public const short DevLB125 = 23125;

        public const short DevLB126 = 23126;

        public const short DevLB127 = 23127;

        public const short DevLB128 = 23128;

        public const short DevLB129 = 23129;

        public const short DevLB130 = 23130;

        public const short DevLB131 = 23131;

        public const short DevLB132 = 23132;

        public const short DevLB133 = 23133;

        public const short DevLB134 = 23134;

        public const short DevLB135 = 23135;

        public const short DevLB136 = 23136;

        public const short DevLB137 = 23137;

        public const short DevLB138 = 23138;

        public const short DevLB139 = 23139;

        public const short DevLB140 = 23140;

        public const short DevLB141 = 23141;

        public const short DevLB142 = 23142;

        public const short DevLB143 = 23143;

        public const short DevLB144 = 23144;

        public const short DevLB145 = 23145;

        public const short DevLB146 = 23146;

        public const short DevLB147 = 23147;

        public const short DevLB148 = 23148;

        public const short DevLB149 = 23149;

        public const short DevLB150 = 23150;

        public const short DevLB151 = 23151;

        public const short DevLB152 = 23152;

        public const short DevLB153 = 23153;

        public const short DevLB154 = 23154;

        public const short DevLB155 = 23155;

        public const short DevLB156 = 23156;

        public const short DevLB157 = 23157;

        public const short DevLB158 = 23158;

        public const short DevLB159 = 23159;

        public const short DevLB160 = 23160;

        public const short DevLB161 = 23161;

        public const short DevLB162 = 23162;

        public const short DevLB163 = 23163;

        public const short DevLB164 = 23164;

        public const short DevLB165 = 23165;

        public const short DevLB166 = 23166;

        public const short DevLB167 = 23167;

        public const short DevLB168 = 23168;

        public const short DevLB169 = 23169;

        public const short DevLB170 = 23170;

        public const short DevLB171 = 23171;

        public const short DevLB172 = 23172;

        public const short DevLB173 = 23173;

        public const short DevLB174 = 23174;

        public const short DevLB175 = 23175;

        public const short DevLB176 = 23176;

        public const short DevLB177 = 23177;

        public const short DevLB178 = 23178;

        public const short DevLB179 = 23179;

        public const short DevLB180 = 23180;

        public const short DevLB181 = 23181;

        public const short DevLB182 = 23182;

        public const short DevLB183 = 23183;

        public const short DevLB184 = 23184;

        public const short DevLB185 = 23185;

        public const short DevLB186 = 23186;

        public const short DevLB187 = 23187;

        public const short DevLB188 = 23188;

        public const short DevLB189 = 23189;

        public const short DevLB190 = 23190;

        public const short DevLB191 = 23191;

        public const short DevLB192 = 23192;

        public const short DevLB193 = 23193;

        public const short DevLB194 = 23194;

        public const short DevLB195 = 23195;

        public const short DevLB196 = 23196;

        public const short DevLB197 = 23197;

        public const short DevLB198 = 23198;

        public const short DevLB199 = 23199;

        public const short DevLB200 = 23200;

        public const short DevLB201 = 23201;

        public const short DevLB202 = 23202;

        public const short DevLB203 = 23203;

        public const short DevLB204 = 23204;

        public const short DevLB205 = 23205;

        public const short DevLB206 = 23206;

        public const short DevLB207 = 23207;

        public const short DevLB208 = 23208;

        public const short DevLB209 = 23209;

        public const short DevLB210 = 23210;

        public const short DevLB211 = 23211;

        public const short DevLB212 = 23212;

        public const short DevLB213 = 23213;

        public const short DevLB214 = 23214;

        public const short DevLB215 = 23215;

        public const short DevLB216 = 23216;

        public const short DevLB217 = 23217;

        public const short DevLB218 = 23218;

        public const short DevLB219 = 23219;

        public const short DevLB220 = 23220;

        public const short DevLB221 = 23221;

        public const short DevLB222 = 23222;

        public const short DevLB223 = 23223;

        public const short DevLB224 = 23224;

        public const short DevLB225 = 23225;

        public const short DevLB226 = 23226;

        public const short DevLB227 = 23227;

        public const short DevLB228 = 23228;

        public const short DevLB229 = 23229;

        public const short DevLB230 = 23230;

        public const short DevLB231 = 23231;

        public const short DevLB232 = 23232;

        public const short DevLB233 = 23233;

        public const short DevLB234 = 23234;

        public const short DevLB235 = 23235;

        public const short DevLB236 = 23236;

        public const short DevLB237 = 23237;

        public const short DevLB238 = 23238;

        public const short DevLB239 = 23239;

        public const short DevLB240 = 23240;

        public const short DevLB241 = 23241;

        public const short DevLB242 = 23242;

        public const short DevLB243 = 23243;

        public const short DevLB244 = 23244;

        public const short DevLB245 = 23245;

        public const short DevLB246 = 23246;

        public const short DevLB247 = 23247;

        public const short DevLB248 = 23248;

        public const short DevLB249 = 23249;

        public const short DevLB250 = 23250;

        public const short DevLB251 = 23251;

        public const short DevLB252 = 23252;

        public const short DevLB253 = 23253;

        public const short DevLB254 = 23254;

        public const short DevLB255 = 23255;

        public const short DevW = 24;

        public const short DevLW1 = 24001;

        public const short DevLW2 = 24002;

        public const short DevLW3 = 24003;

        public const short DevLW4 = 24004;

        public const short DevLW5 = 24005;

        public const short DevLW6 = 24006;

        public const short DevLW7 = 24007;

        public const short DevLW8 = 24008;

        public const short DevLW9 = 24009;

        public const short DevLW10 = 24010;

        public const short DevLW11 = 24011;

        public const short DevLW12 = 24012;

        public const short DevLW13 = 24013;

        public const short DevLW14 = 24014;

        public const short DevLW15 = 24015;

        public const short DevLW16 = 24016;

        public const short DevLW17 = 24017;

        public const short DevLW18 = 24018;

        public const short DevLW19 = 24019;

        public const short DevLW20 = 24020;

        public const short DevLW21 = 24021;

        public const short DevLW22 = 24022;

        public const short DevLW23 = 24023;

        public const short DevLW24 = 24024;

        public const short DevLW25 = 24025;

        public const short DevLW26 = 24026;

        public const short DevLW27 = 24027;

        public const short DevLW28 = 24028;

        public const short DevLW29 = 24029;

        public const short DevLW30 = 24030;

        public const short DevLW31 = 24031;

        public const short DevLW32 = 24032;

        public const short DevLW33 = 24033;

        public const short DevLW34 = 24034;

        public const short DevLW35 = 24035;

        public const short DevLW36 = 24036;

        public const short DevLW37 = 24037;

        public const short DevLW38 = 24038;

        public const short DevLW39 = 24039;

        public const short DevLW40 = 24040;

        public const short DevLW41 = 24041;

        public const short DevLW42 = 24042;

        public const short DevLW43 = 24043;

        public const short DevLW44 = 24044;

        public const short DevLW45 = 24045;

        public const short DevLW46 = 24046;

        public const short DevLW47 = 24047;

        public const short DevLW48 = 24048;

        public const short DevLW49 = 24049;

        public const short DevLW50 = 24050;

        public const short DevLW51 = 24051;

        public const short DevLW52 = 24052;

        public const short DevLW53 = 24053;

        public const short DevLW54 = 24054;

        public const short DevLW55 = 24055;

        public const short DevLW56 = 24056;

        public const short DevLW57 = 24057;

        public const short DevLW58 = 24058;

        public const short DevLW59 = 24059;

        public const short DevLW60 = 24060;

        public const short DevLW61 = 24061;

        public const short DevLW62 = 24062;

        public const short DevLW63 = 24063;

        public const short DevLW64 = 24064;

        public const short DevLW65 = 24065;

        public const short DevLW66 = 24066;

        public const short DevLW67 = 24067;

        public const short DevLW68 = 24068;

        public const short DevLW69 = 24069;

        public const short DevLW70 = 24070;

        public const short DevLW71 = 24071;

        public const short DevLW72 = 24072;

        public const short DevLW73 = 24073;

        public const short DevLW74 = 24074;

        public const short DevLW75 = 24075;

        public const short DevLW76 = 24076;

        public const short DevLW77 = 24077;

        public const short DevLW78 = 24078;

        public const short DevLW79 = 24079;

        public const short DevLW80 = 24080;

        public const short DevLW81 = 24081;

        public const short DevLW82 = 24082;

        public const short DevLW83 = 24083;

        public const short DevLW84 = 24084;

        public const short DevLW85 = 24085;

        public const short DevLW86 = 24086;

        public const short DevLW87 = 24087;

        public const short DevLW88 = 24088;

        public const short DevLW89 = 24089;

        public const short DevLW90 = 24090;

        public const short DevLW91 = 24091;

        public const short DevLW92 = 24092;

        public const short DevLW93 = 24093;

        public const short DevLW94 = 24094;

        public const short DevLW95 = 24095;

        public const short DevLW96 = 24096;

        public const short DevLW97 = 24097;

        public const short DevLW98 = 24098;

        public const short DevLW99 = 24099;

        public const short DevLW100 = 24100;

        public const short DevLW101 = 24101;

        public const short DevLW102 = 24102;

        public const short DevLW103 = 24103;

        public const short DevLW104 = 24104;

        public const short DevLW105 = 24105;

        public const short DevLW106 = 24106;

        public const short DevLW107 = 24107;

        public const short DevLW108 = 24108;

        public const short DevLW109 = 24109;

        public const short DevLW110 = 24110;

        public const short DevLW111 = 24111;

        public const short DevLW112 = 24112;

        public const short DevLW113 = 24113;

        public const short DevLW114 = 24114;

        public const short DevLW115 = 24115;

        public const short DevLW116 = 24116;

        public const short DevLW117 = 24117;

        public const short DevLW118 = 24118;

        public const short DevLW119 = 24119;

        public const short DevLW120 = 24120;

        public const short DevLW121 = 24121;

        public const short DevLW122 = 24122;

        public const short DevLW123 = 24123;

        public const short DevLW124 = 24124;

        public const short DevLW125 = 24125;

        public const short DevLW126 = 24126;

        public const short DevLW127 = 24127;

        public const short DevLW128 = 24128;

        public const short DevLW129 = 24129;

        public const short DevLW130 = 24130;

        public const short DevLW131 = 24131;

        public const short DevLW132 = 24132;

        public const short DevLW133 = 24133;

        public const short DevLW134 = 24134;

        public const short DevLW135 = 24135;

        public const short DevLW136 = 24136;

        public const short DevLW137 = 24137;

        public const short DevLW138 = 24138;

        public const short DevLW139 = 24139;

        public const short DevLW140 = 24140;

        public const short DevLW141 = 24141;

        public const short DevLW142 = 24142;

        public const short DevLW143 = 24143;

        public const short DevLW144 = 24144;

        public const short DevLW145 = 24145;

        public const short DevLW146 = 24146;

        public const short DevLW147 = 24147;

        public const short DevLW148 = 24148;

        public const short DevLW149 = 24149;

        public const short DevLW150 = 24150;

        public const short DevLW151 = 24151;

        public const short DevLW152 = 24152;

        public const short DevLW153 = 24153;

        public const short DevLW154 = 24154;

        public const short DevLW155 = 24155;

        public const short DevLW156 = 24156;

        public const short DevLW157 = 24157;

        public const short DevLW158 = 24158;

        public const short DevLW159 = 24159;

        public const short DevLW160 = 24160;

        public const short DevLW161 = 24161;

        public const short DevLW162 = 24162;

        public const short DevLW163 = 24163;

        public const short DevLW164 = 24164;

        public const short DevLW165 = 24165;

        public const short DevLW166 = 24166;

        public const short DevLW167 = 24167;

        public const short DevLW168 = 24168;

        public const short DevLW169 = 24169;

        public const short DevLW170 = 24170;

        public const short DevLW171 = 24171;

        public const short DevLW172 = 24172;

        public const short DevLW173 = 24173;

        public const short DevLW174 = 24174;

        public const short DevLW175 = 24175;

        public const short DevLW176 = 24176;

        public const short DevLW177 = 24177;

        public const short DevLW178 = 24178;

        public const short DevLW179 = 24179;

        public const short DevLW180 = 24180;

        public const short DevLW181 = 24181;

        public const short DevLW182 = 24182;

        public const short DevLW183 = 24183;

        public const short DevLW184 = 24184;

        public const short DevLW185 = 24185;

        public const short DevLW186 = 24186;

        public const short DevLW187 = 24187;

        public const short DevLW188 = 24188;

        public const short DevLW189 = 24189;

        public const short DevLW190 = 24190;

        public const short DevLW191 = 24191;

        public const short DevLW192 = 24192;

        public const short DevLW193 = 24193;

        public const short DevLW194 = 24194;

        public const short DevLW195 = 24195;

        public const short DevLW196 = 24196;

        public const short DevLW197 = 24197;

        public const short DevLW198 = 24198;

        public const short DevLW199 = 24199;

        public const short DevLW200 = 24200;

        public const short DevLW201 = 24201;

        public const short DevLW202 = 24202;

        public const short DevLW203 = 24203;

        public const short DevLW204 = 24204;

        public const short DevLW205 = 24205;

        public const short DevLW206 = 24206;

        public const short DevLW207 = 24207;

        public const short DevLW208 = 24208;

        public const short DevLW209 = 24209;

        public const short DevLW210 = 24210;

        public const short DevLW211 = 24211;

        public const short DevLW212 = 24212;

        public const short DevLW213 = 24213;

        public const short DevLW214 = 24214;

        public const short DevLW215 = 24215;

        public const short DevLW216 = 24216;

        public const short DevLW217 = 24217;

        public const short DevLW218 = 24218;

        public const short DevLW219 = 24219;

        public const short DevLW220 = 24220;

        public const short DevLW221 = 24221;

        public const short DevLW222 = 24222;

        public const short DevLW223 = 24223;

        public const short DevLW224 = 24224;

        public const short DevLW225 = 24225;

        public const short DevLW226 = 24226;

        public const short DevLW227 = 24227;

        public const short DevLW228 = 24228;

        public const short DevLW229 = 24229;

        public const short DevLW230 = 24230;

        public const short DevLW231 = 24231;

        public const short DevLW232 = 24232;

        public const short DevLW233 = 24233;

        public const short DevLW234 = 24234;

        public const short DevLW235 = 24235;

        public const short DevLW236 = 24236;

        public const short DevLW237 = 24237;

        public const short DevLW238 = 24238;

        public const short DevLW239 = 24239;

        public const short DevLW240 = 24240;

        public const short DevLW241 = 24241;

        public const short DevLW242 = 24242;

        public const short DevLW243 = 24243;

        public const short DevLW244 = 24244;

        public const short DevLW245 = 24245;

        public const short DevLW246 = 24246;

        public const short DevLW247 = 24247;

        public const short DevLW248 = 24248;

        public const short DevLW249 = 24249;

        public const short DevLW250 = 24250;

        public const short DevLW251 = 24251;

        public const short DevLW252 = 24252;

        public const short DevLW253 = 24253;

        public const short DevLW254 = 24254;

        public const short DevLW255 = 24255;

        public const short DevQSB = 25;

        public const short DevLSB1 = 25001;

        public const short DevLSB2 = 25002;

        public const short DevLSB3 = 25003;

        public const short DevLSB4 = 25004;

        public const short DevLSB5 = 25005;

        public const short DevLSB6 = 25006;

        public const short DevLSB7 = 25007;

        public const short DevLSB8 = 25008;

        public const short DevLSB9 = 25009;

        public const short DevLSB10 = 25010;

        public const short DevLSB11 = 25011;

        public const short DevLSB12 = 25012;

        public const short DevLSB13 = 25013;

        public const short DevLSB14 = 25014;

        public const short DevLSB15 = 25015;

        public const short DevLSB16 = 25016;

        public const short DevLSB17 = 25017;

        public const short DevLSB18 = 25018;

        public const short DevLSB19 = 25019;

        public const short DevLSB20 = 25020;

        public const short DevLSB21 = 25021;

        public const short DevLSB22 = 25022;

        public const short DevLSB23 = 25023;

        public const short DevLSB24 = 25024;

        public const short DevLSB25 = 25025;

        public const short DevLSB26 = 25026;

        public const short DevLSB27 = 25027;

        public const short DevLSB28 = 25028;

        public const short DevLSB29 = 25029;

        public const short DevLSB30 = 25030;

        public const short DevLSB31 = 25031;

        public const short DevLSB32 = 25032;

        public const short DevLSB33 = 25033;

        public const short DevLSB34 = 25034;

        public const short DevLSB35 = 25035;

        public const short DevLSB36 = 25036;

        public const short DevLSB37 = 25037;

        public const short DevLSB38 = 25038;

        public const short DevLSB39 = 25039;

        public const short DevLSB40 = 25040;

        public const short DevLSB41 = 25041;

        public const short DevLSB42 = 25042;

        public const short DevLSB43 = 25043;

        public const short DevLSB44 = 25044;

        public const short DevLSB45 = 25045;

        public const short DevLSB46 = 25046;

        public const short DevLSB47 = 25047;

        public const short DevLSB48 = 25048;

        public const short DevLSB49 = 25049;

        public const short DevLSB50 = 25050;

        public const short DevLSB51 = 25051;

        public const short DevLSB52 = 25052;

        public const short DevLSB53 = 25053;

        public const short DevLSB54 = 25054;

        public const short DevLSB55 = 25055;

        public const short DevLSB56 = 25056;

        public const short DevLSB57 = 25057;

        public const short DevLSB58 = 25058;

        public const short DevLSB59 = 25059;

        public const short DevLSB60 = 25060;

        public const short DevLSB61 = 25061;

        public const short DevLSB62 = 25062;

        public const short DevLSB63 = 25063;

        public const short DevLSB64 = 25064;

        public const short DevLSB65 = 25065;

        public const short DevLSB66 = 25066;

        public const short DevLSB67 = 25067;

        public const short DevLSB68 = 25068;

        public const short DevLSB69 = 25069;

        public const short DevLSB70 = 25070;

        public const short DevLSB71 = 25071;

        public const short DevLSB72 = 25072;

        public const short DevLSB73 = 25073;

        public const short DevLSB74 = 25074;

        public const short DevLSB75 = 25075;

        public const short DevLSB76 = 25076;

        public const short DevLSB77 = 25077;

        public const short DevLSB78 = 25078;

        public const short DevLSB79 = 25079;

        public const short DevLSB80 = 25080;

        public const short DevLSB81 = 25081;

        public const short DevLSB82 = 25082;

        public const short DevLSB83 = 25083;

        public const short DevLSB84 = 25084;

        public const short DevLSB85 = 25085;

        public const short DevLSB86 = 25086;

        public const short DevLSB87 = 25087;

        public const short DevLSB88 = 25088;

        public const short DevLSB89 = 25089;

        public const short DevLSB90 = 25090;

        public const short DevLSB91 = 25091;

        public const short DevLSB92 = 25092;

        public const short DevLSB93 = 25093;

        public const short DevLSB94 = 25094;

        public const short DevLSB95 = 25095;

        public const short DevLSB96 = 25096;

        public const short DevLSB97 = 25097;

        public const short DevLSB98 = 25098;

        public const short DevLSB99 = 25099;

        public const short DevLSB100 = 25100;

        public const short DevLSB101 = 25101;

        public const short DevLSB102 = 25102;

        public const short DevLSB103 = 25103;

        public const short DevLSB104 = 25104;

        public const short DevLSB105 = 25105;

        public const short DevLSB106 = 25106;

        public const short DevLSB107 = 25107;

        public const short DevLSB108 = 25108;

        public const short DevLSB109 = 25109;

        public const short DevLSB110 = 25110;

        public const short DevLSB111 = 25111;

        public const short DevLSB112 = 25112;

        public const short DevLSB113 = 25113;

        public const short DevLSB114 = 25114;

        public const short DevLSB115 = 25115;

        public const short DevLSB116 = 25116;

        public const short DevLSB117 = 25117;

        public const short DevLSB118 = 25118;

        public const short DevLSB119 = 25119;

        public const short DevLSB120 = 25120;

        public const short DevLSB121 = 25121;

        public const short DevLSB122 = 25122;

        public const short DevLSB123 = 25123;

        public const short DevLSB124 = 25124;

        public const short DevLSB125 = 25125;

        public const short DevLSB126 = 25126;

        public const short DevLSB127 = 25127;

        public const short DevLSB128 = 25128;

        public const short DevLSB129 = 25129;

        public const short DevLSB130 = 25130;

        public const short DevLSB131 = 25131;

        public const short DevLSB132 = 25132;

        public const short DevLSB133 = 25133;

        public const short DevLSB134 = 25134;

        public const short DevLSB135 = 25135;

        public const short DevLSB136 = 25136;

        public const short DevLSB137 = 25137;

        public const short DevLSB138 = 25138;

        public const short DevLSB139 = 25139;

        public const short DevLSB140 = 25140;

        public const short DevLSB141 = 25141;

        public const short DevLSB142 = 25142;

        public const short DevLSB143 = 25143;

        public const short DevLSB144 = 25144;

        public const short DevLSB145 = 25145;

        public const short DevLSB146 = 25146;

        public const short DevLSB147 = 25147;

        public const short DevLSB148 = 25148;

        public const short DevLSB149 = 25149;

        public const short DevLSB150 = 25150;

        public const short DevLSB151 = 25151;

        public const short DevLSB152 = 25152;

        public const short DevLSB153 = 25153;

        public const short DevLSB154 = 25154;

        public const short DevLSB155 = 25155;

        public const short DevLSB156 = 25156;

        public const short DevLSB157 = 25157;

        public const short DevLSB158 = 25158;

        public const short DevLSB159 = 25159;

        public const short DevLSB160 = 25160;

        public const short DevLSB161 = 25161;

        public const short DevLSB162 = 25162;

        public const short DevLSB163 = 25163;

        public const short DevLSB164 = 25164;

        public const short DevLSB165 = 25165;

        public const short DevLSB166 = 25166;

        public const short DevLSB167 = 25167;

        public const short DevLSB168 = 25168;

        public const short DevLSB169 = 25169;

        public const short DevLSB170 = 25170;

        public const short DevLSB171 = 25171;

        public const short DevLSB172 = 25172;

        public const short DevLSB173 = 25173;

        public const short DevLSB174 = 25174;

        public const short DevLSB175 = 25175;

        public const short DevLSB176 = 25176;

        public const short DevLSB177 = 25177;

        public const short DevLSB178 = 25178;

        public const short DevLSB179 = 25179;

        public const short DevLSB180 = 25180;

        public const short DevLSB181 = 25181;

        public const short DevLSB182 = 25182;

        public const short DevLSB183 = 25183;

        public const short DevLSB184 = 25184;

        public const short DevLSB185 = 25185;

        public const short DevLSB186 = 25186;

        public const short DevLSB187 = 25187;

        public const short DevLSB188 = 25188;

        public const short DevLSB189 = 25189;

        public const short DevLSB190 = 25190;

        public const short DevLSB191 = 25191;

        public const short DevLSB192 = 25192;

        public const short DevLSB193 = 25193;

        public const short DevLSB194 = 25194;

        public const short DevLSB195 = 25195;

        public const short DevLSB196 = 25196;

        public const short DevLSB197 = 25197;

        public const short DevLSB198 = 25198;

        public const short DevLSB199 = 25199;

        public const short DevLSB200 = 25200;

        public const short DevLSB201 = 25201;

        public const short DevLSB202 = 25202;

        public const short DevLSB203 = 25203;

        public const short DevLSB204 = 25204;

        public const short DevLSB205 = 25205;

        public const short DevLSB206 = 25206;

        public const short DevLSB207 = 25207;

        public const short DevLSB208 = 25208;

        public const short DevLSB209 = 25209;

        public const short DevLSB210 = 25210;

        public const short DevLSB211 = 25211;

        public const short DevLSB212 = 25212;

        public const short DevLSB213 = 25213;

        public const short DevLSB214 = 25214;

        public const short DevLSB215 = 25215;

        public const short DevLSB216 = 25216;

        public const short DevLSB217 = 25217;

        public const short DevLSB218 = 25218;

        public const short DevLSB219 = 25219;

        public const short DevLSB220 = 25220;

        public const short DevLSB221 = 25221;

        public const short DevLSB222 = 25222;

        public const short DevLSB223 = 25223;

        public const short DevLSB224 = 25224;

        public const short DevLSB225 = 25225;

        public const short DevLSB226 = 25226;

        public const short DevLSB227 = 25227;

        public const short DevLSB228 = 25228;

        public const short DevLSB229 = 25229;

        public const short DevLSB230 = 25230;

        public const short DevLSB231 = 25231;

        public const short DevLSB232 = 25232;

        public const short DevLSB233 = 25233;

        public const short DevLSB234 = 25234;

        public const short DevLSB235 = 25235;

        public const short DevLSB236 = 25236;

        public const short DevLSB237 = 25237;

        public const short DevLSB238 = 25238;

        public const short DevLSB239 = 25239;

        public const short DevLSB240 = 25240;

        public const short DevLSB241 = 25241;

        public const short DevLSB242 = 25242;

        public const short DevLSB243 = 25243;

        public const short DevLSB244 = 25244;

        public const short DevLSB245 = 25245;

        public const short DevLSB246 = 25246;

        public const short DevLSB247 = 25247;

        public const short DevLSB248 = 25248;

        public const short DevLSB249 = 25249;

        public const short DevLSB250 = 25250;

        public const short DevLSB251 = 25251;

        public const short DevLSB252 = 25252;

        public const short DevLSB253 = 25253;

        public const short DevLSB254 = 25254;

        public const short DevLSB255 = 25255;

        public const short DevSTT = 26;

        public const short DevSTC = 27;

        public const short DevQSW = 28;

        public const short DevLSW1 = 28001;

        public const short DevLSW2 = 28002;

        public const short DevLSW3 = 28003;

        public const short DevLSW4 = 28004;

        public const short DevLSW5 = 28005;

        public const short DevLSW6 = 28006;

        public const short DevLSW7 = 28007;

        public const short DevLSW8 = 28008;

        public const short DevLSW9 = 28009;

        public const short DevLSW10 = 28010;

        public const short DevLSW11 = 28011;

        public const short DevLSW12 = 28012;

        public const short DevLSW13 = 28013;

        public const short DevLSW14 = 28014;

        public const short DevLSW15 = 28015;

        public const short DevLSW16 = 28016;

        public const short DevLSW17 = 28017;

        public const short DevLSW18 = 28018;

        public const short DevLSW19 = 28019;

        public const short DevLSW20 = 28020;

        public const short DevLSW21 = 28021;

        public const short DevLSW22 = 28022;

        public const short DevLSW23 = 28023;

        public const short DevLSW24 = 28024;

        public const short DevLSW25 = 28025;

        public const short DevLSW26 = 28026;

        public const short DevLSW27 = 28027;

        public const short DevLSW28 = 28028;

        public const short DevLSW29 = 28029;

        public const short DevLSW30 = 28030;

        public const short DevLSW31 = 28031;

        public const short DevLSW32 = 28032;

        public const short DevLSW33 = 28033;

        public const short DevLSW34 = 28034;

        public const short DevLSW35 = 28035;

        public const short DevLSW36 = 28036;

        public const short DevLSW37 = 28037;

        public const short DevLSW38 = 28038;

        public const short DevLSW39 = 28039;

        public const short DevLSW40 = 28040;

        public const short DevLSW41 = 28041;

        public const short DevLSW42 = 28042;

        public const short DevLSW43 = 28043;

        public const short DevLSW44 = 28044;

        public const short DevLSW45 = 28045;

        public const short DevLSW46 = 28046;

        public const short DevLSW47 = 28047;

        public const short DevLSW48 = 28048;

        public const short DevLSW49 = 28049;

        public const short DevLSW50 = 28050;

        public const short DevLSW51 = 28051;

        public const short DevLSW52 = 28052;

        public const short DevLSW53 = 28053;

        public const short DevLSW54 = 28054;

        public const short DevLSW55 = 28055;

        public const short DevLSW56 = 28056;

        public const short DevLSW57 = 28057;

        public const short DevLSW58 = 28058;

        public const short DevLSW59 = 28059;

        public const short DevLSW60 = 28060;

        public const short DevLSW61 = 28061;

        public const short DevLSW62 = 28062;

        public const short DevLSW63 = 28063;

        public const short DevLSW64 = 28064;

        public const short DevLSW65 = 28065;

        public const short DevLSW66 = 28066;

        public const short DevLSW67 = 28067;

        public const short DevLSW68 = 28068;

        public const short DevLSW69 = 28069;

        public const short DevLSW70 = 28070;

        public const short DevLSW71 = 28071;

        public const short DevLSW72 = 28072;

        public const short DevLSW73 = 28073;

        public const short DevLSW74 = 28074;

        public const short DevLSW75 = 28075;

        public const short DevLSW76 = 28076;

        public const short DevLSW77 = 28077;

        public const short DevLSW78 = 28078;

        public const short DevLSW79 = 28079;

        public const short DevLSW80 = 28080;

        public const short DevLSW81 = 28081;

        public const short DevLSW82 = 28082;

        public const short DevLSW83 = 28083;

        public const short DevLSW84 = 28084;

        public const short DevLSW85 = 28085;

        public const short DevLSW86 = 28086;

        public const short DevLSW87 = 28087;

        public const short DevLSW88 = 28088;

        public const short DevLSW89 = 28089;

        public const short DevLSW90 = 28090;

        public const short DevLSW91 = 28091;

        public const short DevLSW92 = 28092;

        public const short DevLSW93 = 28093;

        public const short DevLSW94 = 28094;

        public const short DevLSW95 = 28095;

        public const short DevLSW96 = 28096;

        public const short DevLSW97 = 28097;

        public const short DevLSW98 = 28098;

        public const short DevLSW99 = 28099;

        public const short DevLSW100 = 28100;

        public const short DevLSW101 = 28101;

        public const short DevLSW102 = 28102;

        public const short DevLSW103 = 28103;

        public const short DevLSW104 = 28104;

        public const short DevLSW105 = 28105;

        public const short DevLSW106 = 28106;

        public const short DevLSW107 = 28107;

        public const short DevLSW108 = 28108;

        public const short DevLSW109 = 28109;

        public const short DevLSW110 = 28110;

        public const short DevLSW111 = 28111;

        public const short DevLSW112 = 28112;

        public const short DevLSW113 = 28113;

        public const short DevLSW114 = 28114;

        public const short DevLSW115 = 28115;

        public const short DevLSW116 = 28116;

        public const short DevLSW117 = 28117;

        public const short DevLSW118 = 28118;

        public const short DevLSW119 = 28119;

        public const short DevLSW120 = 28120;

        public const short DevLSW121 = 28121;

        public const short DevLSW122 = 28122;

        public const short DevLSW123 = 28123;

        public const short DevLSW124 = 28124;

        public const short DevLSW125 = 28125;

        public const short DevLSW126 = 28126;

        public const short DevLSW127 = 28127;

        public const short DevLSW128 = 28128;

        public const short DevLSW129 = 28129;

        public const short DevLSW130 = 28130;

        public const short DevLSW131 = 28131;

        public const short DevLSW132 = 28132;

        public const short DevLSW133 = 28133;

        public const short DevLSW134 = 28134;

        public const short DevLSW135 = 28135;

        public const short DevLSW136 = 28136;

        public const short DevLSW137 = 28137;

        public const short DevLSW138 = 28138;

        public const short DevLSW139 = 28139;

        public const short DevLSW140 = 28140;

        public const short DevLSW141 = 28141;

        public const short DevLSW142 = 28142;

        public const short DevLSW143 = 28143;

        public const short DevLSW144 = 28144;

        public const short DevLSW145 = 28145;

        public const short DevLSW146 = 28146;

        public const short DevLSW147 = 28147;

        public const short DevLSW148 = 28148;

        public const short DevLSW149 = 28149;

        public const short DevLSW150 = 28150;

        public const short DevLSW151 = 28151;

        public const short DevLSW152 = 28152;

        public const short DevLSW153 = 28153;

        public const short DevLSW154 = 28154;

        public const short DevLSW155 = 28155;

        public const short DevLSW156 = 28156;

        public const short DevLSW157 = 28157;

        public const short DevLSW158 = 28158;

        public const short DevLSW159 = 28159;

        public const short DevLSW160 = 28160;

        public const short DevLSW161 = 28161;

        public const short DevLSW162 = 28162;

        public const short DevLSW163 = 28163;

        public const short DevLSW164 = 28164;

        public const short DevLSW165 = 28165;

        public const short DevLSW166 = 28166;

        public const short DevLSW167 = 28167;

        public const short DevLSW168 = 28168;

        public const short DevLSW169 = 28169;

        public const short DevLSW170 = 28170;

        public const short DevLSW171 = 28171;

        public const short DevLSW172 = 28172;

        public const short DevLSW173 = 28173;

        public const short DevLSW174 = 28174;

        public const short DevLSW175 = 28175;

        public const short DevLSW176 = 28176;

        public const short DevLSW177 = 28177;

        public const short DevLSW178 = 28178;

        public const short DevLSW179 = 28179;

        public const short DevLSW180 = 28180;

        public const short DevLSW181 = 28181;

        public const short DevLSW182 = 28182;

        public const short DevLSW183 = 28183;

        public const short DevLSW184 = 28184;

        public const short DevLSW185 = 28185;

        public const short DevLSW186 = 28186;

        public const short DevLSW187 = 28187;

        public const short DevLSW188 = 28188;

        public const short DevLSW189 = 28189;

        public const short DevLSW190 = 28190;

        public const short DevLSW191 = 28191;

        public const short DevLSW192 = 28192;

        public const short DevLSW193 = 28193;

        public const short DevLSW194 = 28194;

        public const short DevLSW195 = 28195;

        public const short DevLSW196 = 28196;

        public const short DevLSW197 = 28197;

        public const short DevLSW198 = 28198;

        public const short DevLSW199 = 28199;

        public const short DevLSW200 = 28200;

        public const short DevLSW201 = 28201;

        public const short DevLSW202 = 28202;

        public const short DevLSW203 = 28203;

        public const short DevLSW204 = 28204;

        public const short DevLSW205 = 28205;

        public const short DevLSW206 = 28206;

        public const short DevLSW207 = 28207;

        public const short DevLSW208 = 28208;

        public const short DevLSW209 = 28209;

        public const short DevLSW210 = 28210;

        public const short DevLSW211 = 28211;

        public const short DevLSW212 = 28212;

        public const short DevLSW213 = 28213;

        public const short DevLSW214 = 28214;

        public const short DevLSW215 = 28215;

        public const short DevLSW216 = 28216;

        public const short DevLSW217 = 28217;

        public const short DevLSW218 = 28218;

        public const short DevLSW219 = 28219;

        public const short DevLSW220 = 28220;

        public const short DevLSW221 = 28221;

        public const short DevLSW222 = 28222;

        public const short DevLSW223 = 28223;

        public const short DevLSW224 = 28224;

        public const short DevLSW225 = 28225;

        public const short DevLSW226 = 28226;

        public const short DevLSW227 = 28227;

        public const short DevLSW228 = 28228;

        public const short DevLSW229 = 28229;

        public const short DevLSW230 = 28230;

        public const short DevLSW231 = 28231;

        public const short DevLSW232 = 28232;

        public const short DevLSW233 = 28233;

        public const short DevLSW234 = 28234;

        public const short DevLSW235 = 28235;

        public const short DevLSW236 = 28236;

        public const short DevLSW237 = 28237;

        public const short DevLSW238 = 28238;

        public const short DevLSW239 = 28239;

        public const short DevLSW240 = 28240;

        public const short DevLSW241 = 28241;

        public const short DevLSW242 = 28242;

        public const short DevLSW243 = 28243;

        public const short DevLSW244 = 28244;

        public const short DevLSW245 = 28245;

        public const short DevLSW246 = 28246;

        public const short DevLSW247 = 28247;

        public const short DevLSW248 = 28248;

        public const short DevLSW249 = 28249;

        public const short DevLSW250 = 28250;

        public const short DevLSW251 = 28251;

        public const short DevLSW252 = 28252;

        public const short DevLSW253 = 28253;

        public const short DevLSW254 = 28254;

        public const short DevLSW255 = 28255;

        public const short DevSPG0 = 29000;

        public const short DevSPG1 = 29001;

        public const short DevSPG2 = 29002;

        public const short DevSPG3 = 29003;

        public const short DevSPG4 = 29004;

        public const short DevSPG5 = 29005;

        public const short DevSPG6 = 29006;

        public const short DevSPG7 = 29007;

        public const short DevSPG8 = 29008;

        public const short DevSPG9 = 29009;

        public const short DevSPG10 = 29010;

        public const short DevSPG11 = 29011;

        public const short DevSPG12 = 29012;

        public const short DevSPG13 = 29013;

        public const short DevSPG14 = 29014;

        public const short DevSPG15 = 29015;

        public const short DevSPG16 = 29016;

        public const short DevSPG17 = 29017;

        public const short DevSPG18 = 29018;

        public const short DevSPG19 = 29019;

        public const short DevSPG20 = 29020;

        public const short DevSPG21 = 29021;

        public const short DevSPG22 = 29022;

        public const short DevSPG23 = 29023;

        public const short DevSPG24 = 29024;

        public const short DevSPG25 = 29025;

        public const short DevSPG26 = 29026;

        public const short DevSPG27 = 29027;

        public const short DevSPG28 = 29028;

        public const short DevSPG29 = 29029;

        public const short DevSPG30 = 29030;

        public const short DevSPG31 = 29031;

        public const short DevSPG32 = 29032;

        public const short DevSPG33 = 29033;

        public const short DevSPG34 = 29034;

        public const short DevSPG35 = 29035;

        public const short DevSPG36 = 29036;

        public const short DevSPG37 = 29037;

        public const short DevSPG38 = 29038;

        public const short DevSPG39 = 29039;

        public const short DevSPG40 = 29040;

        public const short DevSPG41 = 29041;

        public const short DevSPG42 = 29042;

        public const short DevSPG43 = 29043;

        public const short DevSPG44 = 29044;

        public const short DevSPG45 = 29045;

        public const short DevSPG46 = 29046;

        public const short DevSPG47 = 29047;

        public const short DevSPG48 = 29048;

        public const short DevSPG49 = 29049;

        public const short DevSPG50 = 29050;

        public const short DevSPG51 = 29051;

        public const short DevSPG52 = 29052;

        public const short DevSPG53 = 29053;

        public const short DevSPG54 = 29054;

        public const short DevSPG55 = 29055;

        public const short DevSPG56 = 29056;

        public const short DevSPG57 = 29057;

        public const short DevSPG58 = 29058;

        public const short DevSPG59 = 29059;

        public const short DevSPG60 = 29060;

        public const short DevSPG61 = 29061;

        public const short DevSPG62 = 29062;

        public const short DevSPG63 = 29063;

        public const short DevSPG64 = 29064;

        public const short DevSPG65 = 29065;

        public const short DevSPG66 = 29066;

        public const short DevSPG67 = 29067;

        public const short DevSPG68 = 29068;

        public const short DevSPG69 = 29069;

        public const short DevSPG70 = 29070;

        public const short DevSPG71 = 29071;

        public const short DevSPG72 = 29072;

        public const short DevSPG73 = 29073;

        public const short DevSPG74 = 29074;

        public const short DevSPG75 = 29075;

        public const short DevSPG76 = 29076;

        public const short DevSPG77 = 29077;

        public const short DevSPG78 = 29078;

        public const short DevSPG79 = 29079;

        public const short DevSPG80 = 29080;

        public const short DevSPG81 = 29081;

        public const short DevSPG82 = 29082;

        public const short DevSPG83 = 29083;

        public const short DevSPG84 = 29084;

        public const short DevSPG85 = 29085;

        public const short DevSPG86 = 29086;

        public const short DevSPG87 = 29087;

        public const short DevSPG88 = 29088;

        public const short DevSPG89 = 29089;

        public const short DevSPG90 = 29090;

        public const short DevSPG91 = 29091;

        public const short DevSPG92 = 29092;

        public const short DevSPG93 = 29093;

        public const short DevSPG94 = 29094;

        public const short DevSPG95 = 29095;

        public const short DevSPG96 = 29096;

        public const short DevSPG97 = 29097;

        public const short DevSPG98 = 29098;

        public const short DevSPG99 = 29099;

        public const short DevSPG100 = 29100;

        public const short DevSPG101 = 29101;

        public const short DevSPG102 = 29102;

        public const short DevSPG103 = 29103;

        public const short DevSPG104 = 29104;

        public const short DevSPG105 = 29105;

        public const short DevSPG106 = 29106;

        public const short DevSPG107 = 29107;

        public const short DevSPG108 = 29108;

        public const short DevSPG109 = 29109;

        public const short DevSPG110 = 29110;

        public const short DevSPG111 = 29111;

        public const short DevSPG112 = 29112;

        public const short DevSPG113 = 29113;

        public const short DevSPG114 = 29114;

        public const short DevSPG115 = 29115;

        public const short DevSPG116 = 29116;

        public const short DevSPG117 = 29117;

        public const short DevSPG118 = 29118;

        public const short DevSPG119 = 29119;

        public const short DevSPG120 = 29120;

        public const short DevSPG121 = 29121;

        public const short DevSPG122 = 29122;

        public const short DevSPG123 = 29123;

        public const short DevSPG124 = 29124;

        public const short DevSPG125 = 29125;

        public const short DevSPG126 = 29126;

        public const short DevSPG127 = 29127;

        public const short DevSPG128 = 29128;

        public const short DevSPG129 = 29129;

        public const short DevSPG130 = 29130;

        public const short DevSPG131 = 29131;

        public const short DevSPG132 = 29132;

        public const short DevSPG133 = 29133;

        public const short DevSPG134 = 29134;

        public const short DevSPG135 = 29135;

        public const short DevSPG136 = 29136;

        public const short DevSPG137 = 29137;

        public const short DevSPG138 = 29138;

        public const short DevSPG139 = 29139;

        public const short DevSPG140 = 29140;

        public const short DevSPG141 = 29141;

        public const short DevSPG142 = 29142;

        public const short DevSPG143 = 29143;

        public const short DevSPG144 = 29144;

        public const short DevSPG145 = 29145;

        public const short DevSPG146 = 29146;

        public const short DevSPG147 = 29147;

        public const short DevSPG148 = 29148;

        public const short DevSPG149 = 29149;

        public const short DevSPG150 = 29150;

        public const short DevSPG151 = 29151;

        public const short DevSPG152 = 29152;

        public const short DevSPG153 = 29153;

        public const short DevSPG154 = 29154;

        public const short DevSPG155 = 29155;

        public const short DevSPG156 = 29156;

        public const short DevSPG157 = 29157;

        public const short DevSPG158 = 29158;

        public const short DevSPG159 = 29159;

        public const short DevSPG160 = 29160;

        public const short DevSPG161 = 29161;

        public const short DevSPG162 = 29162;

        public const short DevSPG163 = 29163;

        public const short DevSPG164 = 29164;

        public const short DevSPG165 = 29165;

        public const short DevSPG166 = 29166;

        public const short DevSPG167 = 29167;

        public const short DevSPG168 = 29168;

        public const short DevSPG169 = 29169;

        public const short DevSPG170 = 29170;

        public const short DevSPG171 = 29171;

        public const short DevSPG172 = 29172;

        public const short DevSPG173 = 29173;

        public const short DevSPG174 = 29174;

        public const short DevSPG175 = 29175;

        public const short DevSPG176 = 29176;

        public const short DevSPG177 = 29177;

        public const short DevSPG178 = 29178;

        public const short DevSPG179 = 29179;

        public const short DevSPG180 = 29180;

        public const short DevSPG181 = 29181;

        public const short DevSPG182 = 29182;

        public const short DevSPG183 = 29183;

        public const short DevSPG184 = 29184;

        public const short DevSPG185 = 29185;

        public const short DevSPG186 = 29186;

        public const short DevSPG187 = 29187;

        public const short DevSPG188 = 29188;

        public const short DevSPG189 = 29189;

        public const short DevSPG190 = 29190;

        public const short DevSPG191 = 29191;

        public const short DevSPG192 = 29192;

        public const short DevSPG193 = 29193;

        public const short DevSPG194 = 29194;

        public const short DevSPG195 = 29195;

        public const short DevSPG196 = 29196;

        public const short DevSPG197 = 29197;

        public const short DevSPG198 = 29198;

        public const short DevSPG199 = 29199;

        public const short DevSPG200 = 29200;

        public const short DevSPG201 = 29201;

        public const short DevSPG202 = 29202;

        public const short DevSPG203 = 29203;

        public const short DevSPG204 = 29204;

        public const short DevSPG205 = 29205;

        public const short DevSPG206 = 29206;

        public const short DevSPG207 = 29207;

        public const short DevSPG208 = 29208;

        public const short DevSPG209 = 29209;

        public const short DevSPG210 = 29210;

        public const short DevSPG211 = 29211;

        public const short DevSPG212 = 29212;

        public const short DevSPG213 = 29213;

        public const short DevSPG214 = 29214;

        public const short DevSPG215 = 29215;

        public const short DevSPG216 = 29216;

        public const short DevSPG217 = 29217;

        public const short DevSPG218 = 29218;

        public const short DevSPG219 = 29219;

        public const short DevSPG220 = 29220;

        public const short DevSPG221 = 29221;

        public const short DevSPG222 = 29222;

        public const short DevSPG223 = 29223;

        public const short DevSPG224 = 29224;

        public const short DevSPG225 = 29225;

        public const short DevSPG226 = 29226;

        public const short DevSPG227 = 29227;

        public const short DevSPG228 = 29228;

        public const short DevSPG229 = 29229;

        public const short DevSPG230 = 29230;

        public const short DevSPG231 = 29231;

        public const short DevSPG232 = 29232;

        public const short DevSPG233 = 29233;

        public const short DevSPG234 = 29234;

        public const short DevSPG235 = 29235;

        public const short DevSPG236 = 29236;

        public const short DevSPG237 = 29237;

        public const short DevSPG238 = 29238;

        public const short DevSPG239 = 29239;

        public const short DevSPG240 = 29240;

        public const short DevSPG241 = 29241;

        public const short DevSPG242 = 29242;

        public const short DevSPG243 = 29243;

        public const short DevSPG244 = 29244;

        public const short DevSPG245 = 29245;

        public const short DevSPG246 = 29246;

        public const short DevSPG247 = 29247;

        public const short DevSPG248 = 29248;

        public const short DevSPG249 = 29249;

        public const short DevSPG250 = 29250;

        public const short DevSPG251 = 29251;

        public const short DevSPG252 = 29252;

        public const short DevSPG253 = 29253;

        public const short DevSPG254 = 29254;

        public const short DevSPG255 = 29255;

        public const short DevQV = 30;

        public const short DevMRB = 33;

        public const short DevMAB = 34;

        public const short DevSTN = 35;

        public const short DevWw = 36;

        public const short DevWr = 37;

        public const short DevLZ = 38;

        public const short DevRD = 39;

        public const short DevFS = 40;

        public const short DevLTT = 41;

        public const short DevLTC = 42;

        public const short DevLTN = 43;

        public const short DevLCT = 44;

        public const short DevLCC = 45;

        public const short DevLCN = 46;

        public const short DevLSTT = 47;

        public const short DevLSTC = 48;

        public const short DevLSTN = 49;

        public const short DevSPB = 50;

        public const short DevSPB1 = 501;

        public const short DevSPB2 = 502;

        public const short DevSPB3 = 503;

        public const short DevSPB4 = 504;

        public const short DevSPX = 51;

        public const short DevSPY = 52;

        public const short DevUSER = 100;

        public const short DevMAIL = 101;

        public const short DevMAILNC = 102;

        public const short DevER0 = 22000;

        public const short DevER1 = 22001;

        public const short DevER2 = 22002;

        public const short DevER3 = 22003;

        public const short DevER4 = 22004;

        public const short DevER5 = 22005;

        public const short DevER6 = 22006;

        public const short DevER7 = 22007;

        public const short DevER8 = 22008;

        public const short DevER9 = 22009;

        public const short DevER10 = 22010;

        public const short DevER11 = 22011;

        public const short DevER12 = 22012;

        public const short DevER13 = 22013;

        public const short DevER14 = 22014;

        public const short DevER15 = 22015;

        public const short DevER16 = 22016;

        public const short DevER17 = 22017;

        public const short DevER18 = 22018;

        public const short DevER19 = 22019;

        public const short DevER20 = 22020;

        public const short DevER21 = 22021;

        public const short DevER22 = 22022;

        public const short DevER23 = 22023;

        public const short DevER24 = 22024;

        public const short DevER25 = 22025;

        public const short DevER26 = 22026;

        public const short DevER27 = 22027;

        public const short DevER28 = 22028;

        public const short DevER29 = 22029;

        public const short DevER30 = 22030;

        public const short DevER31 = 22031;

        public const short DevER32 = 22032;

        public const short DevER33 = 22033;

        public const short DevER34 = 22034;

        public const short DevER35 = 22035;

        public const short DevER36 = 22036;

        public const short DevER37 = 22037;

        public const short DevER38 = 22038;

        public const short DevER39 = 22039;

        public const short DevER40 = 22040;

        public const short DevER41 = 22041;

        public const short DevER42 = 22042;

        public const short DevER43 = 22043;

        public const short DevER44 = 22044;

        public const short DevER45 = 22045;

        public const short DevER46 = 22046;

        public const short DevER47 = 22047;

        public const short DevER48 = 22048;

        public const short DevER49 = 22049;

        public const short DevER50 = 22050;

        public const short DevER51 = 22051;

        public const short DevER52 = 22052;

        public const short DevER53 = 22053;

        public const short DevER54 = 22054;

        public const short DevER55 = 22055;

        public const short DevER56 = 22056;

        public const short DevER57 = 22057;

        public const short DevER58 = 22058;

        public const short DevER59 = 22059;

        public const short DevER60 = 22060;

        public const short DevER61 = 22061;

        public const short DevER62 = 22062;

        public const short DevER63 = 22063;

        public const short DevER64 = 22064;

        public const short DevER65 = 22065;

        public const short DevER66 = 22066;

        public const short DevER67 = 22067;

        public const short DevER68 = 22068;

        public const short DevER69 = 22069;

        public const short DevER70 = 22070;

        public const short DevER71 = 22071;

        public const short DevER72 = 22072;

        public const short DevER73 = 22073;

        public const short DevER74 = 22074;

        public const short DevER75 = 22075;

        public const short DevER76 = 22076;

        public const short DevER77 = 22077;

        public const short DevER78 = 22078;

        public const short DevER79 = 22079;

        public const short DevER80 = 22080;

        public const short DevER81 = 22081;

        public const short DevER82 = 22082;

        public const short DevER83 = 22083;

        public const short DevER84 = 22084;

        public const short DevER85 = 22085;

        public const short DevER86 = 22086;

        public const short DevER87 = 22087;

        public const short DevER88 = 22088;

        public const short DevER89 = 22089;

        public const short DevER90 = 22090;

        public const short DevER91 = 22091;

        public const short DevER92 = 22092;

        public const short DevER93 = 22093;

        public const short DevER94 = 22094;

        public const short DevER95 = 22095;

        public const short DevER96 = 22096;

        public const short DevER97 = 22097;

        public const short DevER98 = 22098;

        public const short DevER99 = 22099;

        public const short DevER100 = 22100;

        public const short DevER101 = 22101;

        public const short DevER102 = 22102;

        public const short DevER103 = 22103;

        public const short DevER104 = 22104;

        public const short DevER105 = 22105;

        public const short DevER106 = 22106;

        public const short DevER107 = 22107;

        public const short DevER108 = 22108;

        public const short DevER109 = 22109;

        public const short DevER110 = 22110;

        public const short DevER111 = 22111;

        public const short DevER112 = 22112;

        public const short DevER113 = 22113;

        public const short DevER114 = 22114;

        public const short DevER115 = 22115;

        public const short DevER116 = 22116;

        public const short DevER117 = 22117;

        public const short DevER118 = 22118;

        public const short DevER119 = 22119;

        public const short DevER120 = 22120;

        public const short DevER121 = 22121;

        public const short DevER122 = 22122;

        public const short DevER123 = 22123;

        public const short DevER124 = 22124;

        public const short DevER125 = 22125;

        public const short DevER126 = 22126;

        public const short DevER127 = 22127;

        public const short DevER128 = 22128;

        public const short DevER129 = 22129;

        public const short DevER130 = 22130;

        public const short DevER131 = 22131;

        public const short DevER132 = 22132;

        public const short DevER133 = 22133;

        public const short DevER134 = 22134;

        public const short DevER135 = 22135;

        public const short DevER136 = 22136;

        public const short DevER137 = 22137;

        public const short DevER138 = 22138;

        public const short DevER139 = 22139;

        public const short DevER140 = 22140;

        public const short DevER141 = 22141;

        public const short DevER142 = 22142;

        public const short DevER143 = 22143;

        public const short DevER144 = 22144;

        public const short DevER145 = 22145;

        public const short DevER146 = 22146;

        public const short DevER147 = 22147;

        public const short DevER148 = 22148;

        public const short DevER149 = 22149;

        public const short DevER150 = 22150;

        public const short DevER151 = 22151;

        public const short DevER152 = 22152;

        public const short DevER153 = 22153;

        public const short DevER154 = 22154;

        public const short DevER155 = 22155;

        public const short DevER156 = 22156;

        public const short DevER157 = 22157;

        public const short DevER158 = 22158;

        public const short DevER159 = 22159;

        public const short DevER160 = 22160;

        public const short DevER161 = 22161;

        public const short DevER162 = 22162;

        public const short DevER163 = 22163;

        public const short DevER164 = 22164;

        public const short DevER165 = 22165;

        public const short DevER166 = 22166;

        public const short DevER167 = 22167;

        public const short DevER168 = 22168;

        public const short DevER169 = 22169;

        public const short DevER170 = 22170;

        public const short DevER171 = 22171;

        public const short DevER172 = 22172;

        public const short DevER173 = 22173;

        public const short DevER174 = 22174;

        public const short DevER175 = 22175;

        public const short DevER176 = 22176;

        public const short DevER177 = 22177;

        public const short DevER178 = 22178;

        public const short DevER179 = 22179;

        public const short DevER180 = 22180;

        public const short DevER181 = 22181;

        public const short DevER182 = 22182;

        public const short DevER183 = 22183;

        public const short DevER184 = 22184;

        public const short DevER185 = 22185;

        public const short DevER186 = 22186;

        public const short DevER187 = 22187;

        public const short DevER188 = 22188;

        public const short DevER189 = 22189;

        public const short DevER190 = 22190;

        public const short DevER191 = 22191;

        public const short DevER192 = 22192;

        public const short DevER193 = 22193;

        public const short DevER194 = 22194;

        public const short DevER195 = 22195;

        public const short DevER196 = 22196;

        public const short DevER197 = 22197;

        public const short DevER198 = 22198;

        public const short DevER199 = 22199;

        public const short DevER200 = 22200;

        public const short DevER201 = 22201;

        public const short DevER202 = 22202;

        public const short DevER203 = 22203;

        public const short DevER204 = 22204;

        public const short DevER205 = 22205;

        public const short DevER206 = 22206;

        public const short DevER207 = 22207;

        public const short DevER208 = 22208;

        public const short DevER209 = 22209;

        public const short DevER210 = 22210;

        public const short DevER211 = 22211;

        public const short DevER212 = 22212;

        public const short DevER213 = 22213;

        public const short DevER214 = 22214;

        public const short DevER215 = 22215;

        public const short DevER216 = 22216;

        public const short DevER217 = 22217;

        public const short DevER218 = 22218;

        public const short DevER219 = 22219;

        public const short DevER220 = 22220;

        public const short DevER221 = 22221;

        public const short DevER222 = 22222;

        public const short DevER223 = 22223;

        public const short DevER224 = 22224;

        public const short DevER225 = 22225;

        public const short DevER226 = 22226;

        public const short DevER227 = 22227;

        public const short DevER228 = 22228;

        public const short DevER229 = 22229;

        public const short DevER230 = 22230;

        public const short DevER231 = 22231;

        public const short DevER232 = 22232;

        public const short DevER233 = 22233;

        public const short DevER234 = 22234;

        public const short DevER235 = 22235;

        public const short DevER236 = 22236;

        public const short DevER237 = 22237;

        public const short DevER238 = 22238;

        public const short DevER239 = 22239;

        public const short DevER240 = 22240;

        public const short DevER241 = 22241;

        public const short DevER242 = 22242;

        public const short DevER243 = 22243;

        public const short DevER244 = 22244;

        public const short DevER245 = 22245;

        public const short DevER246 = 22246;

        public const short DevER247 = 22247;

        public const short DevER248 = 22248;

        public const short DevER249 = 22249;

        public const short DevER250 = 22250;

        public const short DevER251 = 22251;

        public const short DevER252 = 22252;

        public const short DevER253 = 22253;

        public const short DevER254 = 22254;

        public const short DevER255 = 22255;

        public const short DevER256 = 22256;

        public const short DevEM0 = 31000;

        public const short DevEM1 = 31001;

        public const short DevEM2 = 31002;

        public const short DevEM3 = 31003;

        public const short DevEM4 = 31004;

        public const short DevEM5 = 31005;

        public const short DevEM6 = 31006;

        public const short DevEM7 = 31007;

        public const short DevEM8 = 31008;

        public const short DevEM9 = 31009;

        public const short DevEM10 = 31010;

        public const short DevEM11 = 31011;

        public const short DevEM12 = 31012;

        public const short DevEM13 = 31013;

        public const short DevEM14 = 31014;

        public const short DevEM15 = 31015;

        public const short DevEM16 = 31016;

        public const short DevEM17 = 31017;

        public const short DevEM18 = 31018;

        public const short DevEM19 = 31019;

        public const short DevEM20 = 31020;

        public const short DevEM21 = 31021;

        public const short DevEM22 = 31022;

        public const short DevEM23 = 31023;

        public const short DevEM24 = 31024;

        public const short DevEM25 = 31025;

        public const short DevEM26 = 31026;

        public const short DevEM27 = 31027;

        public const short DevEM28 = 31028;

        public const short DevEM29 = 31029;

        public const short DevEM30 = 31030;

        public const short DevEM31 = 31031;

        public const short DevEM32 = 31032;

        public const short DevEM33 = 31033;

        public const short DevEM34 = 31034;

        public const short DevEM35 = 31035;

        public const short DevEM36 = 31036;

        public const short DevEM37 = 31037;

        public const short DevEM38 = 31038;

        public const short DevEM39 = 31039;

        public const short DevEM40 = 31040;

        public const short DevEM41 = 31041;

        public const short DevEM42 = 31042;

        public const short DevEM43 = 31043;

        public const short DevEM44 = 31044;

        public const short DevEM45 = 31045;

        public const short DevEM46 = 31046;

        public const short DevEM47 = 31047;

        public const short DevEM48 = 31048;

        public const short DevEM49 = 31049;

        public const short DevEM50 = 31050;

        public const short DevEM51 = 31051;

        public const short DevEM52 = 31052;

        public const short DevEM53 = 31053;

        public const short DevEM54 = 31054;

        public const short DevEM55 = 31055;

        public const short DevEM56 = 31056;

        public const short DevEM57 = 31057;

        public const short DevEM58 = 31058;

        public const short DevEM59 = 31059;

        public const short DevEM60 = 31060;

        public const short DevEM61 = 31061;

        public const short DevEM62 = 31062;

        public const short DevEM63 = 31063;

        public const short DevEM64 = 31064;

        public const short DevEM65 = 31065;

        public const short DevEM66 = 31066;

        public const short DevEM67 = 31067;

        public const short DevEM68 = 31068;

        public const short DevEM69 = 31069;

        public const short DevEM70 = 31070;

        public const short DevEM71 = 31071;

        public const short DevEM72 = 31072;

        public const short DevEM73 = 31073;

        public const short DevEM74 = 31074;

        public const short DevEM75 = 31075;

        public const short DevEM76 = 31076;

        public const short DevEM77 = 31077;

        public const short DevEM78 = 31078;

        public const short DevEM79 = 31079;

        public const short DevEM80 = 31080;

        public const short DevEM81 = 31081;

        public const short DevEM82 = 31082;

        public const short DevEM83 = 31083;

        public const short DevEM84 = 31084;

        public const short DevEM85 = 31085;

        public const short DevEM86 = 31086;

        public const short DevEM87 = 31087;

        public const short DevEM88 = 31088;

        public const short DevEM89 = 31089;

        public const short DevEM90 = 31090;

        public const short DevEM91 = 31091;

        public const short DevEM92 = 31092;

        public const short DevEM93 = 31093;

        public const short DevEM94 = 31094;

        public const short DevEM95 = 31095;

        public const short DevEM96 = 31096;

        public const short DevEM97 = 31097;

        public const short DevEM98 = 31098;

        public const short DevEM99 = 31099;

        public const short DevEM100 = 31100;

        public const short DevEM101 = 31101;

        public const short DevEM102 = 31102;

        public const short DevEM103 = 31103;

        public const short DevEM104 = 31104;

        public const short DevEM105 = 31105;

        public const short DevEM106 = 31106;

        public const short DevEM107 = 31107;

        public const short DevEM108 = 31108;

        public const short DevEM109 = 31109;

        public const short DevEM110 = 31110;

        public const short DevEM111 = 31111;

        public const short DevEM112 = 31112;

        public const short DevEM113 = 31113;

        public const short DevEM114 = 31114;

        public const short DevEM115 = 31115;

        public const short DevEM116 = 31116;

        public const short DevEM117 = 31117;

        public const short DevEM118 = 31118;

        public const short DevEM119 = 31119;

        public const short DevEM120 = 31120;

        public const short DevEM121 = 31121;

        public const short DevEM122 = 31122;

        public const short DevEM123 = 31123;

        public const short DevEM124 = 31124;

        public const short DevEM125 = 31125;

        public const short DevEM126 = 31126;

        public const short DevEM127 = 31127;

        public const short DevEM128 = 31128;

        public const short DevEM129 = 31129;

        public const short DevEM130 = 31130;

        public const short DevEM131 = 31131;

        public const short DevEM132 = 31132;

        public const short DevEM133 = 31133;

        public const short DevEM134 = 31134;

        public const short DevEM135 = 31135;

        public const short DevEM136 = 31136;

        public const short DevEM137 = 31137;

        public const short DevEM138 = 31138;

        public const short DevEM139 = 31139;

        public const short DevEM140 = 31140;

        public const short DevEM141 = 31141;

        public const short DevEM142 = 31142;

        public const short DevEM143 = 31143;

        public const short DevEM144 = 31144;

        public const short DevEM145 = 31145;

        public const short DevEM146 = 31146;

        public const short DevEM147 = 31147;

        public const short DevEM148 = 31148;

        public const short DevEM149 = 31149;

        public const short DevEM150 = 31150;

        public const short DevEM151 = 31151;

        public const short DevEM152 = 31152;

        public const short DevEM153 = 31153;

        public const short DevEM154 = 31154;

        public const short DevEM155 = 31155;

        public const short DevEM156 = 31156;

        public const short DevEM157 = 31157;

        public const short DevEM158 = 31158;

        public const short DevEM159 = 31159;

        public const short DevEM160 = 31160;

        public const short DevEM161 = 31161;

        public const short DevEM162 = 31162;

        public const short DevEM163 = 31163;

        public const short DevEM164 = 31164;

        public const short DevEM165 = 31165;

        public const short DevEM166 = 31166;

        public const short DevEM167 = 31167;

        public const short DevEM168 = 31168;

        public const short DevEM169 = 31169;

        public const short DevEM170 = 31170;

        public const short DevEM171 = 31171;

        public const short DevEM172 = 31172;

        public const short DevEM173 = 31173;

        public const short DevEM174 = 31174;

        public const short DevEM175 = 31175;

        public const short DevEM176 = 31176;

        public const short DevEM177 = 31177;

        public const short DevEM178 = 31178;

        public const short DevEM179 = 31179;

        public const short DevEM180 = 31180;

        public const short DevEM181 = 31181;

        public const short DevEM182 = 31182;

        public const short DevEM183 = 31183;

        public const short DevEM184 = 31184;

        public const short DevEM185 = 31185;

        public const short DevEM186 = 31186;

        public const short DevEM187 = 31187;

        public const short DevEM188 = 31188;

        public const short DevEM189 = 31189;

        public const short DevEM190 = 31190;

        public const short DevEM191 = 31191;

        public const short DevEM192 = 31192;

        public const short DevEM193 = 31193;

        public const short DevEM194 = 31194;

        public const short DevEM195 = 31195;

        public const short DevEM196 = 31196;

        public const short DevEM197 = 31197;

        public const short DevEM198 = 31198;

        public const short DevEM199 = 31199;

        public const short DevEM200 = 31200;

        public const short DevEM201 = 31201;

        public const short DevEM202 = 31202;

        public const short DevEM203 = 31203;

        public const short DevEM204 = 31204;

        public const short DevEM205 = 31205;

        public const short DevEM206 = 31206;

        public const short DevEM207 = 31207;

        public const short DevEM208 = 31208;

        public const short DevEM209 = 31209;

        public const short DevEM210 = 31210;

        public const short DevEM211 = 31211;

        public const short DevEM212 = 31212;

        public const short DevEM213 = 31213;

        public const short DevEM214 = 31214;

        public const short DevEM215 = 31215;

        public const short DevEM216 = 31216;

        public const short DevEM217 = 31217;

        public const short DevEM218 = 31218;

        public const short DevEM219 = 31219;

        public const short DevEM220 = 31220;

        public const short DevEM221 = 31221;

        public const short DevEM222 = 31222;

        public const short DevEM223 = 31223;

        public const short DevEM224 = 31224;

        public const short DevEM225 = 31225;

        public const short DevEM226 = 31226;

        public const short DevEM227 = 31227;

        public const short DevEM228 = 31228;

        public const short DevEM229 = 31229;

        public const short DevEM230 = 31230;

        public const short DevEM231 = 31231;

        public const short DevEM232 = 31232;

        public const short DevEM233 = 31233;

        public const short DevEM234 = 31234;

        public const short DevEM235 = 31235;

        public const short DevEM236 = 31236;

        public const short DevEM237 = 31237;

        public const short DevEM238 = 31238;

        public const short DevEM239 = 31239;

        public const short DevEM240 = 31240;

        public const short DevEM241 = 31241;

        public const short DevEM242 = 31242;

        public const short DevEM243 = 31243;

        public const short DevEM244 = 31244;

        public const short DevEM245 = 31245;

        public const short DevEM246 = 31246;

        public const short DevEM247 = 31247;

        public const short DevEM248 = 31248;

        public const short DevEM249 = 31249;

        public const short DevEM250 = 31250;

        public const short DevEM251 = 31251;

        public const short DevEM252 = 31252;

        public const short DevEM253 = 31253;

        public const short DevEM254 = 31254;

        public const short DevEM255 = 31255;

        public const short DevED0 = 32000;

        public const short DevED1 = 32001;

        public const short DevED2 = 32002;

        public const short DevED3 = 32003;

        public const short DevED4 = 32004;

        public const short DevED5 = 32005;

        public const short DevED6 = 32006;

        public const short DevED7 = 32007;

        public const short DevED8 = 32008;

        public const short DevED9 = 32009;

        public const short DevED10 = 32010;

        public const short DevED11 = 32011;

        public const short DevED12 = 32012;

        public const short DevED13 = 32013;

        public const short DevED14 = 32014;

        public const short DevED15 = 32015;

        public const short DevED16 = 32016;

        public const short DevED17 = 32017;

        public const short DevED18 = 32018;

        public const short DevED19 = 32019;

        public const short DevED20 = 32020;

        public const short DevED21 = 32021;

        public const short DevED22 = 32022;

        public const short DevED23 = 32023;

        public const short DevED24 = 32024;

        public const short DevED25 = 32025;

        public const short DevED26 = 32026;

        public const short DevED27 = 32027;

        public const short DevED28 = 32028;

        public const short DevED29 = 32029;

        public const short DevED30 = 32030;

        public const short DevED31 = 32031;

        public const short DevED32 = 32032;

        public const short DevED33 = 32033;

        public const short DevED34 = 32034;

        public const short DevED35 = 32035;

        public const short DevED36 = 32036;

        public const short DevED37 = 32037;

        public const short DevED38 = 32038;

        public const short DevED39 = 32039;

        public const short DevED40 = 32040;

        public const short DevED41 = 32041;

        public const short DevED42 = 32042;

        public const short DevED43 = 32043;

        public const short DevED44 = 32044;

        public const short DevED45 = 32045;

        public const short DevED46 = 32046;

        public const short DevED47 = 32047;

        public const short DevED48 = 32048;

        public const short DevED49 = 32049;

        public const short DevED50 = 32050;

        public const short DevED51 = 32051;

        public const short DevED52 = 32052;

        public const short DevED53 = 32053;

        public const short DevED54 = 32054;

        public const short DevED55 = 32055;

        public const short DevED56 = 32056;

        public const short DevED57 = 32057;

        public const short DevED58 = 32058;

        public const short DevED59 = 32059;

        public const short DevED60 = 32060;

        public const short DevED61 = 32061;

        public const short DevED62 = 32062;

        public const short DevED63 = 32063;

        public const short DevED64 = 32064;

        public const short DevED65 = 32065;

        public const short DevED66 = 32066;

        public const short DevED67 = 32067;

        public const short DevED68 = 32068;

        public const short DevED69 = 32069;

        public const short DevED70 = 32070;

        public const short DevED71 = 32071;

        public const short DevED72 = 32072;

        public const short DevED73 = 32073;

        public const short DevED74 = 32074;

        public const short DevED75 = 32075;

        public const short DevED76 = 32076;

        public const short DevED77 = 32077;

        public const short DevED78 = 32078;

        public const short DevED79 = 32079;

        public const short DevED80 = 32080;

        public const short DevED81 = 32081;

        public const short DevED82 = 32082;

        public const short DevED83 = 32083;

        public const short DevED84 = 32084;

        public const short DevED85 = 32085;

        public const short DevED86 = 32086;

        public const short DevED87 = 32087;

        public const short DevED88 = 32088;

        public const short DevED89 = 32089;

        public const short DevED90 = 32090;

        public const short DevED91 = 32091;

        public const short DevED92 = 32092;

        public const short DevED93 = 32093;

        public const short DevED94 = 32094;

        public const short DevED95 = 32095;

        public const short DevED96 = 32096;

        public const short DevED97 = 32097;

        public const short DevED98 = 32098;

        public const short DevED99 = 32099;

        public const short DevED100 = 32100;

        public const short DevED101 = 32101;

        public const short DevED102 = 32102;

        public const short DevED103 = 32103;

        public const short DevED104 = 32104;

        public const short DevED105 = 32105;

        public const short DevED106 = 32106;

        public const short DevED107 = 32107;

        public const short DevED108 = 32108;

        public const short DevED109 = 32109;

        public const short DevED110 = 32110;

        public const short DevED111 = 32111;

        public const short DevED112 = 32112;

        public const short DevED113 = 32113;

        public const short DevED114 = 32114;

        public const short DevED115 = 32115;

        public const short DevED116 = 32116;

        public const short DevED117 = 32117;

        public const short DevED118 = 32118;

        public const short DevED119 = 32119;

        public const short DevED120 = 32120;

        public const short DevED121 = 32121;

        public const short DevED122 = 32122;

        public const short DevED123 = 32123;

        public const short DevED124 = 32124;

        public const short DevED125 = 32125;

        public const short DevED126 = 32126;

        public const short DevED127 = 32127;

        public const short DevED128 = 32128;

        public const short DevED129 = 32129;

        public const short DevED130 = 32130;

        public const short DevED131 = 32131;

        public const short DevED132 = 32132;

        public const short DevED133 = 32133;

        public const short DevED134 = 32134;

        public const short DevED135 = 32135;

        public const short DevED136 = 32136;

        public const short DevED137 = 32137;

        public const short DevED138 = 32138;

        public const short DevED139 = 32139;

        public const short DevED140 = 32140;

        public const short DevED141 = 32141;

        public const short DevED142 = 32142;

        public const short DevED143 = 32143;

        public const short DevED144 = 32144;

        public const short DevED145 = 32145;

        public const short DevED146 = 32146;

        public const short DevED147 = 32147;

        public const short DevED148 = 32148;

        public const short DevED149 = 32149;

        public const short DevED150 = 32150;

        public const short DevED151 = 32151;

        public const short DevED152 = 32152;

        public const short DevED153 = 32153;

        public const short DevED154 = 32154;

        public const short DevED155 = 32155;

        public const short DevED156 = 32156;

        public const short DevED157 = 32157;

        public const short DevED158 = 32158;

        public const short DevED159 = 32159;

        public const short DevED160 = 32160;

        public const short DevED161 = 32161;

        public const short DevED162 = 32162;

        public const short DevED163 = 32163;

        public const short DevED164 = 32164;

        public const short DevED165 = 32165;

        public const short DevED166 = 32166;

        public const short DevED167 = 32167;

        public const short DevED168 = 32168;

        public const short DevED169 = 32169;

        public const short DevED170 = 32170;

        public const short DevED171 = 32171;

        public const short DevED172 = 32172;

        public const short DevED173 = 32173;

        public const short DevED174 = 32174;

        public const short DevED175 = 32175;

        public const short DevED176 = 32176;

        public const short DevED177 = 32177;

        public const short DevED178 = 32178;

        public const short DevED179 = 32179;

        public const short DevED180 = 32180;

        public const short DevED181 = 32181;

        public const short DevED182 = 32182;

        public const short DevED183 = 32183;

        public const short DevED184 = 32184;

        public const short DevED185 = 32185;

        public const short DevED186 = 32186;

        public const short DevED187 = 32187;

        public const short DevED188 = 32188;

        public const short DevED189 = 32189;

        public const short DevED190 = 32190;

        public const short DevED191 = 32191;

        public const short DevED192 = 32192;

        public const short DevED193 = 32193;

        public const short DevED194 = 32194;

        public const short DevED195 = 32195;

        public const short DevED196 = 32196;

        public const short DevED197 = 32197;

        public const short DevED198 = 32198;

        public const short DevED199 = 32199;

        public const short DevED200 = 32200;

        public const short DevED201 = 32201;

        public const short DevED202 = 32202;

        public const short DevED203 = 32203;

        public const short DevED204 = 32204;

        public const short DevED205 = 32205;

        public const short DevED206 = 32206;

        public const short DevED207 = 32207;

        public const short DevED208 = 32208;

        public const short DevED209 = 32209;

        public const short DevED210 = 32210;

        public const short DevED211 = 32211;

        public const short DevED212 = 32212;

        public const short DevED213 = 32213;

        public const short DevED214 = 32214;

        public const short DevED215 = 32215;

        public const short DevED216 = 32216;

        public const short DevED217 = 32217;

        public const short DevED218 = 32218;

        public const short DevED219 = 32219;

        public const short DevED220 = 32220;

        public const short DevED221 = 32221;

        public const short DevED222 = 32222;

        public const short DevED223 = 32223;

        public const short DevED224 = 32224;

        public const short DevED225 = 32225;

        public const short DevED226 = 32226;

        public const short DevED227 = 32227;

        public const short DevED228 = 32228;

        public const short DevED229 = 32229;

        public const short DevED230 = 32230;

        public const short DevED231 = 32231;

        public const short DevED232 = 32232;

        public const short DevED233 = 32233;

        public const short DevED234 = 32234;

        public const short DevED235 = 32235;

        public const short DevED236 = 32236;

        public const short DevED237 = 32237;

        public const short DevED238 = 32238;

        public const short DevED239 = 32239;

        public const short DevED240 = 32240;

        public const short DevED241 = 32241;

        public const short DevED242 = 32242;

        public const short DevED243 = 32243;

        public const short DevED244 = 32244;

        public const short DevED245 = 32245;

        public const short DevED246 = 32246;

        public const short DevED247 = 32247;

        public const short DevED248 = 32248;

        public const short DevED249 = 32249;

        public const short DevED250 = 32250;

        public const short DevED251 = 32251;

        public const short DevED252 = 32252;

        public const short DevED253 = 32253;

        public const short DevED254 = 32254;

        public const short DevED255 = 32255;

        public const short DevRBM = -32768;

        public const short DevRAB = -32736;

        public const short DevRX = -32735;

        public const short DevRY = -32734;

        public const short DevRW = -32732;

        public const short DevARB = -32704;

        public const short DevSB = -32669;

        public const short DevSW = -32668;

        public int cv_Path = 0;
        public short cv_Buf = 0;
        public short Open()
        {
            //return mdOpen(151, -1, ref cv_Path);
            return 0;
        }
        public short mdBdLedRead()
        {
            //mdBdLedRead(cv_Path, ref cv_Buf);
            //return cv_Buf;
            return 0;
        }
    }
}
