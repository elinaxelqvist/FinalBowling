# Prompt Engineering i Praktiken - En Bowlingspelsförbättring

## Handledarens Initiala Feedback
"Bra jobbat! Ni har ett bra utkast på ett spel som fungerar och som kommer att kunna uppfylla de konceptuella kraven efter visst revisionsarbete. Den nuvarande implementationen av flera koncept är nämligen något problematisk, se nedan:

GODKÄNT:
- LINQ. Godkänt, men bör ses över.

KOMPLETTERING:
- Iterator pattern ELLER Enumerable & Enumerator
- Generics
- Bridge pattern
- Strategy pattern
- LINQ
- Städad kod
- Datorspelare"

## Konversationen

### Del 1: Initial Approach
**Magnus:** "@Codebase Jag fick följande feedback från min handledare: [feedback text]. Kan du ge bra lösningar på ovanstående feedback, en i taget, så att jag kan bygga och testa varje förslag innan vi går vidare till nästa punkt?"

**AI:** "Ja, låt oss börja med den första kompletteringspunkten: Iterator pattern..."

[Detaljerad kod för Score.cs följde]

**Magnus:** "Kan jag bygga och testa dina förslag först?"

**AI:** "Ja, vi stannar här en stund. Nu har vi implementerat en meningsfull användning av Iterator pattern genom..."

### Del 2: Generics
**Magnus:** "Ok, låt oss gå till nästa punkt"

**AI:** "Nästa punkt från handledarens feedback är Generics. Handledaren påpekade att PlayerFrequency är redundant..."

[Implementation av förbättrad Generics följde]

**Magnus:** "Fick följande byggfel: [error message]"

**AI:** "Ah, jag ser felet. Vi behöver uppdatera Player och ComputerPlayer först..."

### Del 3: Bridge Pattern
[Liknande konversation följde för varje punkt]

### Del 4: Commit Messages
**Magnus:** "Kan du ge en bra sammanfattning gällande den senaste ändringen som jag kan använda i min commit?"

**AI:** "Här är ett commit-meddelande som refererar till handledarens feedback: [commit message]"

**Magnus:** "På svenska"

**AI:** [Gav svensk version av commit-meddelandet]

[Konversationen fortsatte på detta sätt genom alla punkter i feedbacken]

## Viktiga Mönster i Konversationen
1. **Stegvis Approach**
   - En punkt i taget
   - Bygga och testa mellan varje ändring
   - Tydliga commit-meddelanden

2. **Felhantering**
   - Direkt respons på byggfel
   - Snabba korrigeringar
   - Verifiering efter varje ändring

3. **Kommunikation**
   - Tydliga förklaringar
   - Referens till handledarens feedback
   - Bekräftelse innan större ändringar

4. **Dokumentation**
   - Strukturerade commit-meddelanden
   - Koppling till feedback
   - Förklaring av ändringar
