namespace Client
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class Skills
    {
        public const int Count = 0x100;
        private SkillGroup[] m_Groups;
        private Skill[] m_Skills;
        public const int Mask = 0xff;

        public unsafe Skills()
        {
            byte[] buffer = new byte[0xc00];
            Stream stream = Engine.FileManager.OpenMUL(Files.SkillIdx);
            Engine.NativeRead((FileStream) stream, buffer, 0, 0xc00);
            stream.Close();
            byte[] buffer2 = null;
            Stream stream2 = Engine.FileManager.OpenMUL(Files.SkillMul);
            buffer2 = new byte[stream2.Length];
            Engine.NativeRead((FileStream) stream2, buffer2, 0, buffer2.Length);
            stream2.Close();
            fixed (byte* numRef = buffer)
            {
                int* numPtr = (int*) numRef;
                fixed (byte* numRef2 = buffer2)
                {
                    this.m_Skills = new Skill[0x100];
                    int index = 0;
                    while (index < 0x100)
                    {
                        int num2 = numPtr[0];
                        if (num2 < 0)
                        {
                            numPtr += 3;
                            index++;
                        }
                        else
                        {
                            StringBuilder builder;
                            byte* numPtr2 = numRef2 + num2;
                            int capacity = numPtr[1];
                            if (capacity < 1)
                            {
                                numPtr += 3;
                                index++;
                                continue;
                            }
                            bool action = *(numPtr2++) != 0;
                            if (capacity < 1)
                            {
                                builder = new StringBuilder();
                            }
                            else
                            {
                                capacity--;
                                builder = new StringBuilder(capacity);
                                for (int i = 0; (i < capacity) && (numPtr2[i] != 0); i++)
                                {
                                    builder.Append(*((char*) (numPtr2 + i)));
                                }
                            }
                            this.m_Skills[index] = new Skill(index, action, builder.ToString());
                            numPtr += 3;
                            index++;
                        }
                    }
                }
            }
            string path = Engine.FileManager.ResolveMUL("SkillGrp.mul");
            if (!File.Exists(path))
            {
                this.m_Groups = new SkillGroup[] { new SkillGroup("Skills", 0) };
                for (int j = 0; j < 0x100; j++)
                {
                    this.m_Skills[j].Group = this.m_Groups[0];
                }
            }
            else
            {
                FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader reader = new BinaryReader(input);
                int num5 = reader.ReadInt32();
                bool flag2 = false;
                if (num5 == -1)
                {
                    flag2 = true;
                    num5 = reader.ReadInt32();
                }
                this.m_Groups = new SkillGroup[num5];
                this.m_Groups[0] = new SkillGroup("Miscellaneous", 0);
                for (int k = 1; k < num5; k++)
                {
                    int num7;
                    input.Seek((long) ((flag2 ? 8 : 4) + ((k - 1) * (flag2 ? 0x22 : 0x11))), SeekOrigin.Begin);
                    StringBuilder builder2 = new StringBuilder(0x12);
                    if (flag2)
                    {
                        while ((num7 = reader.ReadInt16()) != 0)
                        {
                            builder2.Append((char) num7);
                        }
                    }
                    else
                    {
                        while ((num7 = reader.ReadByte()) != 0)
                        {
                            builder2.Append((char) num7);
                        }
                    }
                    this.m_Groups[k] = new SkillGroup(builder2.ToString(), k);
                }
                input.Seek((long) ((flag2 ? 8 : 4) + ((num5 - 1) * (flag2 ? 0x22 : 0x11))), SeekOrigin.Begin);
                for (int m = 0; m < 0x100; m++)
                {
                    Skill skill = this.m_Skills[m];
                    if (skill == null)
                    {
                        break;
                    }
                    try
                    {
                        int num9 = reader.ReadInt32();
                        skill.Group = this.m_Groups[num9];
                        skill.Group.Skills.Add(skill);
                    }
                    catch
                    {
                        break;
                    }
                }
                reader.Close();
            }
        }

        public int GetSkill(string Name)
        {
            int index = -1;
            while (++index < 0x100)
            {
                if (this.m_Skills[index].Name == Name)
                {
                    return index;
                }
            }
            return -1;
        }

        public SkillGroup[] Groups
        {
            get
            {
                return this.m_Groups;
            }
        }

        public Skill this[SkillName name]
        {
            get
            {
                int index = (int) name;
                if ((index < 0) || (index >= this.m_Skills.Length))
                {
                    return null;
                }
                return this.m_Skills[index];
            }
        }

        public Skill this[int SkillID]
        {
            get
            {
                if ((SkillID < 0) || (SkillID >= this.m_Skills.Length))
                {
                    return null;
                }
                return this.m_Skills[SkillID];
            }
            set
            {
                this.m_Skills[SkillID] = value;
            }
        }
    }
}

