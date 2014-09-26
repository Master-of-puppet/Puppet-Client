using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet
{
    public static class Fields
    {
        public const string USERNAME            = "userName";
        public const string PASSWORD            = "password";

        public const string DETAILS             = "details";
        public const string SUCCESS             = "success";
        public const string TOKEN               = "token";
        public const string DATA                = "data";
        public const string PARAMS              = "params";
        public const string MESSAGE             = "message";
        public const string CMD                 = "cmd";

        public const string COMMAND_GET_CHIDREN  = "getChildren";
        public const string COMMAND_GET_GROUP_CHILDREN = "getGroupChildren";
        public const string COMMAND_CREATE_GAME = "createGame";

        public const string REQUEST_JOIN_ROOM = "joinRoomRequest";
        public const string REQUEST_PLUGIN = "pluginRequest";

        public const string RESPONSE_FIRST_ROOM_TO_JOIN = "firstRoomToJoin";

        public const string RESPONSE_CMD_PLUGIN_MESSAGE = "pluginMessage";
    }
}
