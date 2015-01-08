namespace Client
{
    using System;
    using System.Collections;

    public class CharacterComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            CharacterProfile character = null;
            CharacterProfile profile2 = null;
            if (x is GPlayCharacterMenu)
            {
                character = ((GPlayCharacterMenu) x).Character;
            }
            else
            {
                character = x as CharacterProfile;
            }
            if (y is GPlayCharacterMenu)
            {
                profile2 = ((GPlayCharacterMenu) y).Character;
            }
            else
            {
                profile2 = y as CharacterProfile;
            }
            if (character == null)
            {
                return 1;
            }
            if (profile2 == null)
            {
                return -1;
            }
            return (character.Index - profile2.Index);
        }
    }
}

