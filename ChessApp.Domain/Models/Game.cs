namespace ChessApp.Domain.Models
{
    public class Game
    {
        public int Id { get; set; } // идентификатор игры
        public int PlayerId { get; set; } // идентификатор игрока
        public string Memo { get; set; } // доп поле комментарий к игре
        public string Result { get; set; } // результат игры : победа, поражение, ничья
        public Player Player { get; set; } // игрок
    }
}
