using System;
using System.Collections.Generic;
using System.Text;

namespace HOKProtocol {
    [Serializable]
    public enum KeyType {
        None,
        Move,
        Skill,
        //TOADD
    }
    [Serializable]
    public class OpKey {
        public int opIndex;
        public KeyType keyType;
        public SkillKey skillKey;
        public MoveKey moveKey;
        //TOADD
    }

    [Serializable]
    public class SkillKey {
        public uint skillID;

        public long x_value;
        public long z_value;
    }

    [Serializable]
    public class MoveKey {
        //debug
        public uint keyID;

        public long x;
        public long z;
    }
}
