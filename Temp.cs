using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ChatApp_DB
{
    static class Temp
    {
        public static int globalid;
        public static string reciever;
        public static List<User> users = new List<User>();
        public static List<ChatRoom> chatrooms = new List<ChatRoom>();
        public static List<Contact> contacts = new List<Contact>();

        public static bool done = false;

        public static int[] roomIDs = new int[10];

        public static int chatroomid;
        public static string roomname;

        public static bool loadedrooms = false;
    }
}
