namespace RadioArchive.Maui
{
    public static class IconTypeExtenstions
    {
        /// <summary>
        /// Converts <see cref="IconType"/> to FontAwesome string
        /// </summary>
        /// <param name="type">the type to convert to</param>
        /// <returns></returns>
        public static string ToFontAwesome(this IconType type)
        {
            //return the FontAwsome string base on given type
            switch (type)
            {
                case IconType.Picture:
                    return "\uf0f6";
                case IconType.File:
                    return "\uf1c5";
                case IconType.Morning:
                    return "\uf6c4";
                case IconType.Evening:
                    return "\uf186";
                case IconType.Afternoon:
                    return "\uf185";
                case IconType.Podcast:
                    return "\uf2ce";
                case IconType.Play:
                    return "\uf04b";
                case IconType.Pause:
                    return "\uf04c";
                case IconType.Heart:
                    return "\uf004";               
                case IconType.History:
                    return "\uf1da";
                case IconType.Add:
                    return "\uf055";
                case IconType.UserList:
                    return "\uf022";
                case IconType.Share:
                    return "\uf064";
                case IconType.Download:
                    return "\uf019";
                case IconType.MoreVertical:
                    return "\uf142";
                case IconType.RightSide:
                    return "\uf105"; 
                case IconType.Remove:
                    return "\uf1f8";  
                case IconType.Edit:
                    return "\uf044";
                case IconType.Copy:
                    return "\uf0c5";
                //if none found return null
                default:
                    return null;
            }
        }
    }
}
