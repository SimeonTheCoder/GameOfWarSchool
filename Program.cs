using GameOfWar;
using System.Collections;
using System.Linq;

class Program
{
    static Queue<Card> firstPlayerDeck;
    static Queue<Card> secondPlayerDeck;

    static Card firstPlayerCard;
    static Card secondPlayerCard;
    
    static void TitleScreen()
    {
        Console.WriteLine("\r\n================================================================================\r\n||                     Welcome to the Game of War!                            ||\r\n||                                                                            ||\r\n|| HOW TO PLAY:                                                               ||\r\n|| + Each of the two players are dealt one half of a shuffled deck of cards.  ||\r\n|| + Each turn, each player draws one card from their deck.                   ||\r\n|| + The player that drew the card with higher value gets both cards.         ||\r\n|| + Both cards return to the winner's deck.                                  ||\r\n|| + If there is a draw, both players place the next three cards face down    ||\r\n||        and then another card face-up. The owner of the higher face-up      ||\r\n||        card gets all the cards on the table.                               ||\r\n||                                                                            ||\r\n|| HOW TO WIN:                                                                ||\r\n|| + The player who collects all the cards wins.                              ||\r\n||                                                                            ||\r\n|| CONTROLS:                                                                  ||\r\n|| + Press [Enter] to draw a new card until we have a winner.                 ||\r\n||                                                                            ||\r\n||                              Have fun!                                     ||\r\n================================================================================\r\n");
    }
    static List<Card> GenerateDeck()
    {
        List<Card> deck = new List<Card>();

        CardFace[] faces = (CardFace[])Enum.GetValues(typeof(CardFace));
        CardSuit[] suits = (CardSuit[])Enum.GetValues(typeof(CardSuit));

        for(int suite = 0; suite < suits.Length; suite++)
        {
            for(int face = 0; face < faces.Length; face++)
            {
                CardFace currentFace = faces[face];
                CardSuit currentSuit = suits[suite];

                deck.Add(new Card
                    {
                        Face = currentFace,
                        Suite = currentSuit
                    });
            }
        }

        return deck;
    }

    static void Shuffle(List<Card> deck)
    {
        Random random = new Random();

        for(int i = 0; i < deck.Count; i ++)
        {
            int firstCardIndex = random.Next(deck.Count);

            Card tempCard = deck[firstCardIndex];

            deck[firstCardIndex] = deck[i];
            deck[i] = tempCard;
        }
    }

    static void DealCardsToPlayers(List<Card> deck)
    {
        while(deck.Count > 0)
        {
            Card[] firstTwoDrawnCards = deck.Take(2).ToArray();
            deck.RemoveRange(0, 2);

            firstPlayerDeck.Enqueue(firstTwoDrawnCards[0]);
            secondPlayerDeck.Enqueue(firstTwoDrawnCards[1]);
        }
    }

    static bool GameHasWinner(int totalMoves)
    {
        if(!firstPlayerDeck.Any())
        {
            Console.WriteLine($"After a total of {totalMoves} moves, the second player has won!");
            return true;
        }
        else if(!secondPlayerDeck.Any())
        {
            Console.WriteLine($"After a total of {totalMoves} moves, the first player has won!");
            return true;
        }

        return false;
    }

    static void DrawPlayerCards()
    {
        firstPlayerCard = firstPlayerDeck.Dequeue();
        secondPlayerCard = secondPlayerDeck.Dequeue();

        Console.WriteLine(firstPlayerCard);
        Console.WriteLine(secondPlayerCard);
    }

    static void ProcessWar(Queue<Card> pool)
    {
        try
        {
            while ((int)firstPlayerCard.Face == (int)secondPlayerCard.Face)
            {
                Console.WriteLine("WAR!");

                if (firstPlayerDeck.Count < 4)
                {
                    AddCardsToWinnerDeck(firstPlayerDeck, secondPlayerDeck);
                    Console.WriteLine($"First player does not have enough cards to continue playing...");
                    break;
                }

                if (secondPlayerDeck.Count < 4)
                {
                    AddCardsToWinnerDeck(secondPlayerDeck, firstPlayerDeck);
                    Console.WriteLine($"Second player does not have enough cards to continue playing...");
                    break;
                }

                AddWarCardsToPool(secondPlayerDeck);

                firstPlayerCard = firstPlayerDeck.Dequeue();
                secondPlayerCard = secondPlayerDeck.Dequeue();

                Console.WriteLine($"First player has drawn: {firstPlayerCard}");
                Console.WriteLine($"Second player has drawn: {secondPlayerCard}");

                pool.Enqueue(firstPlayerCard);
                pool.Enqueue(secondPlayerCard);
            }
        }catch(Exception exception)
        {
            Console.WriteLine();
        }
    }

    static void DetermineWinner(Queue<Card> pool)
    {
        if((int) firstPlayerCard.Face > (int) secondPlayerCard.Face)
        {
            Console.WriteLine("The first player has won the cards!");

            foreach(Card card in pool)
            {
                if(card == null)
                {
                    Console.WriteLine();
                }
                firstPlayerDeck.Enqueue(card);
            }
        }
        else
        {
            Console.WriteLine("The second player has won the cards!");

            foreach (Card card in pool)
            {
                if (card == null)
                {
                    Console.WriteLine();
                }
                secondPlayerDeck.Enqueue(card);
            }
        }
    }

    static void AddWarCardsToPool(Queue<Card> pool)
    {
        for(int i = 0; i < 3; i++)
        {
            pool.Enqueue(firstPlayerDeck.Dequeue());
            pool.Enqueue(secondPlayerDeck.Dequeue());
        }
    }

    static void AddCardsToWinnerDeck(Queue<Card> loserDeck, Queue<Card> winnerDeck)
    {
        while(loserDeck.Count > 0)
        {
            loserDeck.Enqueue(winnerDeck.Dequeue());
        }
    }

    static void Main(string[] args)
    {
        TitleScreen();

        List<Card> deck = GenerateDeck();
        Shuffle(deck);

        firstPlayerDeck = new Queue<Card>();
        secondPlayerDeck = new Queue<Card>();

        DealCardsToPlayers(deck);

        firstPlayerCard = null;
        secondPlayerCard = null;

        int totalMoves = 0;

        while(!GameHasWinner(totalMoves))
        {
            Console.ReadLine();
            DrawPlayerCards();

            Queue<Card> pool = new Queue<Card>();

            pool.Enqueue(firstPlayerCard);
            pool.Enqueue(secondPlayerCard);
            
            ProcessWar(pool);
            DetermineWinner(pool);

            Console.WriteLine("========================================");
            Console.WriteLine($"First player currently has {firstPlayerDeck.Count} cards.");
            Console.WriteLine($"Second player currently has {secondPlayerDeck.Count} cards.");
            Console.WriteLine("========================================");

            totalMoves++;
        }
    }
}