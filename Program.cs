using System;
using System.Threading;

public class Program
{

    private static Timer wekkerTimer;
    private static TimerCallback timerCallback;

    public static void Main()
    {
        //Declareren van variablen ivm met de timer
        int wektijd = 30;
        int sluimtijd = 30;
        int optie;
        bool alarm = true;
        bool boodschap = true;
        bool knipperlicht = false;

        //Het toevoegen van de methoden aan de delegate
        timerCallback = TimerDisplayBoodschap;
        timerCallback += TimerAlarm;

        //Het instellen van de timer met de delegate als callback methode
        wekkerTimer = new Timer(timerCallback, null, wektijd * 1000, sluimtijd * 1000);

        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("_-====={ Menu wekker: }======-_" +
                "\n1: wekker instellen" +
                "\n2: sluimertijd instellen" +
                "\n3: stop wekker & sluimer" +
                "\n-=- Opties wekken: -=-" +
                "\n4: schakel wekkeralarm aan/uit" +
                "\n5: schakel boodschap aan/uit" +
                "\n6: schakel knipperlicht aan/uit (warning flashing lights)");

            //Lees de invoer en stel de keuze gelijk aan een niet bestaande optie als de invoer geen nummer is
            Console.ForegroundColor = ConsoleColor.White;
            if (!int.TryParse(Console.ReadLine(), out optie))
            {
                optie = -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            switch (optie)
            {
                case 1:
                    Console.WriteLine($"Wekker staat momenteel ingesteld op {wektijd}s. Voer een nieuwe wektijd in seconden in: ");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (int.TryParse(Console.ReadLine(), out wektijd)) //controleer of invoer een int is
                    {
                        //Update de timer naar nieuwe wektijd
                        wekkerTimer.Change(wektijd * 1000, sluimtijd * 1000);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Wektijd succesvol ingesteld op {wektijd} seconden.");
                    }
                    else
                    {
                        //Foutbericht bij ongeldige invoer
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ongeldige invoer voor wektijd.");
                    }
                    break;

                case 2:
                    Console.WriteLine($"Sluimtijd staat momenteel in4" +
                        $"gesteld op {sluimtijd}s. Voer een nieuwe sluimtijd in seconden in: ");

                    Console.ForegroundColor = ConsoleColor.White;
                    if (int.TryParse(Console.ReadLine(), out sluimtijd))
                    {
                        //Update de timer naar nieuwe sluimtijd
                        wekkerTimer.Change(wektijd * 1000, sluimtijd * 1000);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Sluimtijd succesvol ingesteld op {sluimtijd} seconden.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ongeldige invoer voor sluimtijd.");
                    }

                    break;

                case 3:
                    //De wekker op stop leggen door de wektijd & sluimtijd op infinite te zetten
                    wekkerTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Wekker succesvol gestopt");
                    break;

                case 4:
                    //De methode van de delegate toevoegen / weghalen afhankelijk van de bool alarm
                    if (alarm)
                    {
                        timerCallback -= TimerAlarm;
                        Console.WriteLine("- timeralarm");
                    }
                    else
                    {
                        timerCallback += TimerAlarm;
                        Console.WriteLine("+ timeralarm");
                    }
                    //De timer opnieuw declareren, zodat de delegates geupdated worden
                    updateTimer(wektijd * 1000, sluimtijd * 1000);

                    //De bool omkeren
                    alarm = !alarm;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Wekkeralarm succesvol " + (alarm ? "ingeschakeld" : "uitgeschakeld"));
                    break;

                case 5:
                    if (boodschap)
                    {
                        timerCallback -= TimerDisplayBoodschap;
                    }
                    else
                    {
                        timerCallback += TimerDisplayBoodschap;
                    }
                    updateTimer(wektijd * 1000, sluimtijd * 1000);

                    boodschap = !boodschap;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Boodschap wekker succesvol " + (boodschap ? "ingeschakeld" : "uitgeschakeld"));
                    break;

                case 6:
                    if (knipperlicht)
                    {
                        timerCallback -= KnipperLicht;
                    }
                    else
                    {
                        timerCallback += KnipperLicht;
                    }
                    updateTimer(wektijd * 1000, sluimtijd * 1000);

                    knipperlicht = !knipperlicht;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Knipperlicht wekker succesvol " + (knipperlicht ? "ingeschakeld" : "uitgeschakeld"));
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Optie niet herkend");
                    break;
            }

        } while (true);
    }

    static void TimerDisplayBoodschap(object state)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("ALARM AAN HET AFGAAN");

        Console.ForegroundColor = ConsoleColor.White;
    }

    static void TimerAlarm(object state)
    {
        Console.Beep();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("(*WEKKERGELUIDEN*)");
    }

    static void KnipperLicht(object state)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.WriteLine("*KNIPPERREN*");
        for (int i = 0; i < 10; i++)
        {
            Console.Clear();
            Console.BackgroundColor = (i % 2 == 0) ? ConsoleColor.Black : ConsoleColor.White;
            Console.WriteLine("*ALARM AAN HET AFGAAN MET KNIPPERLICHTEN*");
            Thread.Sleep(300);
        }

        Console.BackgroundColor = ConsoleColor.Black;
    }

    //Een methode om de timer up te daten (nieuwe declareren) als de delegate gewijzigd werd
    private static void updateTimer(int wektijdMs, int sluimTijdMs)
    {
        //De wektijd van de timer blijvan laten tikken (oneindig) zodat deze instantie niet de alarm blijft laten uitvoeren, (voor de nieuwe te declareren)
        wekkerTimer.Change(Timeout.Infinite, Timeout.Infinite);
        //De timer opnieuw declareren met de timerCallBack
        wekkerTimer = new Timer(timerCallback, null, wektijdMs, sluimTijdMs);
    }

}