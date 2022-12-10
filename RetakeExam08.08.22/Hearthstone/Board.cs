using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Board : IBoard
{
    private Dictionary<string, Card> deck = new Dictionary<string, Card>();

    public bool Contains(string name)
    {
        return this.deck.ContainsKey(name);
    }

    public int Count()
    {
        return this.deck.Count;
    }

    public void Draw(Card card)
    {
        if (this.deck.ContainsKey(card.Name))
        {
            throw new ArgumentException();
        }

        this.deck.Add(card.Name, card);
    }

    public IEnumerable<Card> GetBestInRange(int start, int end)
    {
        return this.deck.Values.Where(card => card.Score >= start && card.Score <= end)
                    .OrderByDescending(card => card.Level);
    }

    public void Heal(int health)
    {
        deck.Values.OrderBy(card => card.Health).FirstOrDefault().Health += health;

    }

    public IEnumerable<Card> ListCardsByPrefix(string prefix)
    {
        return this.deck.Values
                        .Where(card => card.Name.StartsWith(prefix))
                        .OrderBy(card => string.Join("", card.Name.Reverse()))
                        .ThenBy(card => card.Level);
    }

    public void Play(string attackerCardName, string defenderCardName)
    {
        if (!this.deck.ContainsKey(attackerCardName) || !this.deck.ContainsKey(defenderCardName))
        {
            throw new ArgumentException();
        }

        var attacker = this.deck[attackerCardName];
        var defender = this.deck[defenderCardName];

        if (attacker.Level != defender.Level)
        {
            throw new ArgumentException();
        }

        if (defender.Health <= 0 || attacker.Health <= 0)
        {
            return;
        }

        defender.Health -= attacker.Damage;

        if (defender.Health <= 0)
        {
            attacker.Score += defender.Level;
        }
    }

    public void Remove(string name)
    {
        if (!this.deck.ContainsKey(name))
        {
            throw new ArgumentException();
        }

        this.deck.Remove(name);
    }

    public void RemoveDeath()
    {
        var removed = deck.Values.Where(c => c.Health <= 0).ToArray();

        foreach (var card in removed)
        {
            this.deck.Remove(card.Name);
        }

        //this.deck = this.deck.Where(kvp => kvp.Value.Health <= 0).ToDictionary(x => x.Key, x => x.Value);
    }

    public IEnumerable<Card> SearchByLevel(int level)
    {
        return this.deck.Values.Where(card => card.Level == level).OrderByDescending(card => card.Score);
    }
}