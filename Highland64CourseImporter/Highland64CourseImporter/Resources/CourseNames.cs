﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter
{
    public static class CourseNames
    {
        public static int[] OrderedIndices = new int[] {
            36,//Toad Highlands
            48,
            38,
            51,
            2,
            41,
            42,
            1,
            44,
            45,
            46,
            47,
            37,
            49,
            50,
            39,
            52,
            53,
            0, //Koopa Park
            43,
            40,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            58, //Shy Guy Desert
            62,
            66,
            55,
            61,
            60,
            63,
            71,
            69,
            59,
            56,
            64,
            70,
            67,
            65,
            57,
            68,
            54,
            72, //Yoshi's Island
            73,
            74,
            75,
            76,
            77,
            78,
            79,
            80,
            81,
            82,
            83,
            84,
            85,
            86,
            87,
            88,
            89,
            18, //Boo Valley
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            92, //Mario's Star
            106,
            101,
            93,
            97,
            102,
            95,
            99,
            91,
            105,
            90,
            100,
            96,
            94,
            98,
            104,
            103,
            107,
            108, //Luigi's Garden
            109,
            110,
            111,
            112,
            113,
            114,
            115,
            116,
            117,
            118,
            119,
            120,
            121,
            122,
            123,
            124,
            125,
            126, //Peach's Castle
            127,
            128,
            129,
            130,
            131,
            132,
            133,
            134,
            135,
            136,
            137,
            138,
            139,
            140,
            141,
            142,
            143,
            144 //Intro Preview Course
        };

        public static string[] NameList = new string[] {
            "Koopa Park Hole 1", //1
            "Toad Highlands Hole 8",
            "Toad Highlands Hole 5",
            "Koopa Park Hole 4",
            "Koopa Park Hole 5",
            "Koopa Park Hole 6",
            "Koopa Park Hole 7",
            "Koopa Park Hole 8",
            "Koopa Park Hole 9",
            "Koopa Park Hole 10",
            "Koopa Park Hole 11",
            "Koopa Park Hole 12",
            "Koopa Park Hole 13",
            "Koopa Park Hole 14",
            "Koopa Park Hole 15",
            "Koopa Park Hole 16",
            "Koopa Park Hole 17",
            "Koopa Park Hole 18", //18
            "Boo Valley Hole 1",
            "Boo Valley Hole 2",
            "Boo Valley Hole 3",
            "Boo Valley Hole 4",
            "Boo Valley Hole 5",
            "Boo Valley Hole 6",
            "Boo Valley Hole 7",
            "Boo Valley Hole 8",
            "Boo Valley Hole 9",
            "Boo Valley Hole 10",
            "Boo Valley Hole 11",
            "Boo Valley Hole 12",
            "Boo Valley Hole 13",
            "Boo Valley Hole 14",
            "Boo Valley Hole 15",
            "Boo Valley Hole 16",
            "Boo Valley Hole 17",
            "Boo Valley Hole 18", //36
            "Toad Highlands Hole 1",
            "Toad Highlands Hole 13",
            "Toad Highlands Hole 3",
            "Toad Highlands Hole 16",
            "Koopa Park Hole 3",
            "Toad Highlands Hole 6",
            "Toad Highlands Hole 7",
            "Koopa Park Hole 2",
            "Toad Highlands Hole 9",
            "Toad Highlands Hole 10",
            "Toad Highlands Hole 11",
            "Toad Highlands Hole 12",
            "Toad Highlands Hole 2",
            "Toad Highlands Hole 14",
            "Toad Highlands Hole 15",
            "Toad Highlands Hole 4",
            "Toad Highlands Hole 17",
            "Toad Highlands Hole 18", //54
            "Shy Guy Desert Hole 18",
            "Shy Guy Desert Hole 4",
            "Shy Guy Desert Hole 11",
            "Shy Guy Desert Hole 16",
            "Shy Guy Desert Hole 1",
            "Shy Guy Desert Hole 10",
            "Shy Guy Desert Hole 6",
            "Shy Guy Desert Hole 5",
            "Shy Guy Desert Hole 2",
            "Shy Guy Desert Hole 7",
            "Shy Guy Desert Hole 12",
            "Shy Guy Desert Hole 15",
            "Shy Guy Desert Hole 3",
            "Shy Guy Desert Hole 14",
            "Shy Guy Desert Hole 17",
            "Shy Guy Desert Hole 9",
            "Shy Guy Desert Hole 13",
            "Shy Guy Desert Hole 8", //72
            "Yoshi's Island Hole 1",
            "Yoshi's Island Hole 2",
            "Yoshi's Island Hole 3",
            "Yoshi's Island Hole 4",
            "Yoshi's Island Hole 5",
            "Yoshi's Island Hole 6",
            "Yoshi's Island Hole 7",
            "Yoshi's Island Hole 8",
            "Yoshi's Island Hole 9",
            "Yoshi's Island Hole 10",
            "Yoshi's Island Hole 11",
            "Yoshi's Island Hole 12",
            "Yoshi's Island Hole 13",
            "Yoshi's Island Hole 14",
            "Yoshi's Island Hole 15",
            "Yoshi's Island Hole 16",
            "Yoshi's Island Hole 17",
            "Yoshi's Island Hole 18", //90
            "Mario's Star Hole 11",
            "Mario's Star Hole 9",
            "Mario's Star Hole 1",
            "Mario's Star Hole 4",
            "Mario's Star Hole 14",
            "Mario's Star Hole 7",
            "Mario's Star Hole 13",
            "Mario's Star Hole 5",
            "Mario's Star Hole 15",
            "Mario's Star Hole 8",
            "Mario's Star Hole 12",
            "Mario's Star Hole 3",
            "Mario's Star Hole 6",
            "Mario's Star Hole 17",
            "Mario's Star Hole 16",
            "Mario's Star Hole 10",
            "Mario's Star Hole 2",
            "Mario's Star Hole 18", //108
            "Luigi's Garden Hole 1",
            "Luigi's Garden Hole 2",
            "Luigi's Garden Hole 3",
            "Luigi's Garden Hole 4",
            "Luigi's Garden Hole 5",
            "Luigi's Garden Hole 6",
            "Luigi's Garden Hole 7",
            "Luigi's Garden Hole 8",
            "Luigi's Garden Hole 9",
            "Luigi's Garden Hole 10",
            "Luigi's Garden Hole 11",
            "Luigi's Garden Hole 12",
            "Luigi's Garden Hole 13",
            "Luigi's Garden Hole 14",
            "Luigi's Garden Hole 15",
            "Luigi's Garden Hole 16",
            "Luigi's Garden Hole 17",
            "Luigi's Garden Hole 18", //126
            "Peach's Castle Hole 1",
            "Peach's Castle Hole 2",
            "Peach's Castle Hole 3",
            "Peach's Castle Hole 4",
            "Peach's Castle Hole 5",
            "Peach's Castle Hole 6",
            "Peach's Castle Hole 7",
            "Peach's Castle Hole 8",
            "Peach's Castle Hole 9",
            "Peach's Castle Hole 10",
            "Peach's Castle Hole 11",
            "Peach's Castle Hole 12",
            "Peach's Castle Hole 13",
            "Peach's Castle Hole 14",
            "Peach's Castle Hole 15",
            "Peach's Castle Hole 16",
            "Peach's Castle Hole 17",
            "Peach's Castle Hole 18", //144
            "Intro Preview Course"

        };
    }
}