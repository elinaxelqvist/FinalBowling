# Bowling Game Improvements

Vi har förbättrat bowlingspelet enligt handledarens feedback genom att:

## 1. Bridge Pattern (Throw.cs)
- Förbättrat integrationen mellan Power och Strategy
- Lagt till meningsfulla skillnader mellan Weak och Strong power
- Weak ger bättre kontroll men lägre maxpoäng
- Strong ger högre risk men också chans till högre poäng

## 2. Strategy Pattern (Throw.cs)
- Varje strategi har nu unika egenskaper:
  * LeftHandSpin: Trippel poäng för käglor på vänstra sidan
  * RightHandSpin: Trippel poäng för käglor på högra sidan
  * SuperSpin: Fyrdubbel poäng för käglor i bakre raden
- Olika träffchanser (70% för LeftHandSpin, 60% för övriga)

## 3. Datorspelare (ComputerPlayer.cs)
- Implementerat intelligent strategi för datorn
- Analyserar kägelpositioner före varje kast
- Väljer strategi baserat på var käglorna finns
- Anpassar power efter antal kvarvarande käglor

## 4. Statistikhantering (Statistics.cs, Game.cs)
- Förbättrad hantering av spelstatistik
- Sparar high scores i JSON-format (gamehistory.json)
- Håller reda på antal spelade spel per spelare (gamesplayed.json)
- Visar top 3-lista efter varje spel

## 5. Kodförbättringar (Player.cs, Game.cs, .gitignore, .gitattributes)
- Städat upp onödig kod och parametrar
- Förbättrat LINQ-användningen i statistikhanteringen
- Lagt till tydligare felmeddelanden
- Standardiserat line endings för bättre versionshantering

Spelet är nu mer strategiskt och ger spelaren fler meningsfulla val!
