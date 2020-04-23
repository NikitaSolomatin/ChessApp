using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessApp.Domain.Models
{
    public class Player
    {
        public Player() { Games = new Collection<Game>(); } //конструктор   
        public int Id { get; set; } // идентификатор игрока
        public string FullName { get; set; } // полное имя игрока
        public ICollection<Game> Games { get; set; } // сыгранные игры

    }
}
