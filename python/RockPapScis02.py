# (c) This software is developed by: J Kok
# (c) Date: 1st September 2021 Time: 13.45
# (c) File Name: RockPapScis02.py Version: 2.0


# Rock, Paper, Scissors game

import random
import datetime


# Player class
class Player:
    def __init__(self, name, start_score):
        self.name = name
        self.choice = ""
        self.score = start_score
        self.round_winner = False

    def make_choice(self):
        pass


# Inherits from Player class. Method asks human player to input their choice
class HumanPlayer(Player):
    def make_choice(self):
        valid_input = False
        user_input = ""

        while not valid_input:
            user_input = input("Enter your choice. R = Rock, P = Paper, S = Scissors \n")
            user_input = user_input.upper()  # convert to uppercase

            if (user_input == "R") or (user_input == "P") or (user_input == "S"):
                valid_input = True
            else:
                print("Invalid entry.")

        if user_input == "R":
            self.choice = "Rock"

        if user_input == "P":
            self.choice = "Paper"

        if user_input == "S":
            self.choice = "Scissors"


# Inherits from Player class. Method generates a random choice for the computer player
class ComputerPlayer(Player):
    def make_choice(self):
        random_number = random.randint(0, 2)

        if random_number == 0:
            self.choice = "Rock"

        if random_number == 1:
            self.choice = "Paper"

        if random_number == 2:
            self.choice = "Scissors"


# Game class
class Game:
    def __init__(self):
        self.players = []
        self.num_rounds = 0
        self.winner = ""

    # Asks user to input a number in given range & validates input
    def input_number(self, prompt, maximum):
        valid_input = False
        num = ""

        while not valid_input:
            user_input = input(prompt)

            # test if input is a number
            if not user_input.isnumeric():
                print("Invalid entry")
                continue

            # test if input is within range
            num = int(user_input)
            if (num < 1) or (num > maximum):
                print("Number out of range")
                continue

            valid_input = True

        return num

    def start(self):
        print("\nWelcome to Rock, Paper, Scissors!")
        user_name = input("Please enter your name: ")
        user_name = user_name.upper()

        # create a human player and computer player, then add to game
        player1 = HumanPlayer(user_name, 0)
        player2 = ComputerPlayer("Computer", 0)

        self.players.append(player1)
        self.players.append(player2)

        # Ask user for number of rounds to play
        self.num_rounds = self.input_number("\nHow many rounds do you wish to play? (1-5) ", 5)
        print("\n------------------------------------------------------------------ ")

    def play_round(self, round_number):
        print("\nROUND " + str(round_number + 1))

        # Get choices from each player
        for p in self.players:
            p.round_winner = False
            print("\n" + p.name + "'s turn")
            p.make_choice()

        # Show the player's choices
        print()
        for p in self.players:
            print(p.name + " chose " + p.choice)

        # Determine winner
        if self.players[0].choice == self.players[1].choice:      # Same choice = draw
            print("Result is a draw.")

        elif self.players[0].choice == "Rock":            # User chose Rock
            if self.players[1].choice == "Scissors":
                print("Rock beats scissors.")
                self.players[0].round_winner = True
            elif self.players[1].choice == "Paper":
                print("Paper beats rock.")
                self.players[1].round_winner = True

        elif self.players[0].choice == "Scissors":        # User chose Scissors
            if self.players[1].choice == "Paper":
                print("Scissors beats paper.")
                self.players[0].round_winner = True
            elif self.players[1].choice == "Rock":
                print("Rock beats scissors.")
                self.players[1].round_winner = True

        elif self.players[0].choice == "Paper":           # User chose Paper
            if self.players[1].choice == "Rock":
                print("Paper beats rock.")
                self.players[0].round_winner = True
            elif self.players[1].choice == "Scissors":
                print("Scissors beats paper.")
                self.players[1].round_winner = True

        # Declare winner and increment score
        for p in self.players:
            if p.round_winner:
                print(p.name + " wins the round.")
                p.score += 1

        print("\n------------------------------------------------------------------ ")

    def end_game(self):
        print("\nGame complete")

        for p in self.players:
            print(p.name + "'s score = " + str(p.score))

        if self.players[0].score > self.players[1].score:
            print(self.players[0].name + " is the overall winner!")
            self.winner = self.players[0].name
        elif self.players[1].score > self.players[0].score:
            print(self.players[1].name + " is the overall winner!")
            self.winner = self.players[1].name
        elif self.players[0].score == self.players[1].score:
            print("Scores are tied!")
            self.winner = "Tie"

        print("Thank you for playing!")

    def save_results(self):
        save_filename = input("\nPlease enter name of file to save result: ") + ".txt"

        with open(save_filename, 'w') as savefile:
            savefile.write("Rock, Paper, Scissors game result\n")
            savefile.write(str(datetime.datetime.now()) + "\n \n")
            savefile.write("Number of rounds played: " + str(self.num_rounds) + "\n")

            for p in self.players:
                savefile.write(p.name + "'s score = " + str(p.score) + "\n")

            savefile.write("Winner: " + self.winner)
            savefile.close()

        print("Result saved to file " + save_filename)

        with open(save_filename, 'r') as savefile:
            print("\n" + savefile.read())
            savefile.close()


# Execution starts here ----------------------------
G = Game()
G.start()

for n in range(G.num_rounds):
    G.play_round(n)

G.end_game()
G.save_results()
