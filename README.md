<h1 align="center">ESET Parser Task 👨‍💻 </h1> <br>
<p>We've scanned a random set of both clean and infected files by commandline version of our antivirus engine.
You can find a log from this scanning in attachment 'ESET_Parser attachment.log'.</p>
<h1 align="center"> ✒️Zadanie úlohy: </h1>
<i>Your task is to write a parser for these files, process them, and output list of infected files in the log (example - the very first file 'g:\Scan\Data\1.td') in a human readable form (exact formatting is up to you) 
  with additional information<br>
- whether the file is archive (contains embedded files/objects) (example above: yes)<br>
- list of all unique infiltrations for the file (example above: Win32/Induc virus,a variant of Win32/TrojanDownloader.Delf.PWL trojan,a variant of Win32/Induc.A virus)<br>
- in the case of archive, list of all used packers (example above: NSIS)</i>

<h1 align="center"> 🔧Použitá technológia: </h1>

<div align="center">
  <img src="https://raw.githubusercontent.com/devicons/devicon/1119b9f84c0290e0f0b38982099a2bd027a48bf1/icons/csharp/csharp-original.svg" title="C#" alt="C#" width="40" height="40"/>&nbsp;
</div>

<h1 align="center"> ✍️ Môj komentár k zadaniu: </h1>
<p>🥇 Podľa môjho porozumenia, mojou úlohou bolo vytvoriť program v jazyku C# alebo Python, ktorý bude vyhľadávať infikované súbory v zdrojovom súbore s názvom "ESET_Parser attachment.log". 
  Tieto infikované súbory by mali byť vypísané na konzolu v špecifikovanom formáte, ktorý som si mohol navrhnúť. 
</p>
<p>🥈 
Pre splnenie tejto úlohy som si vybral jazyk C#, pretože mám s ním väčšie skúsenosti. Na vývoj som použil IDE "Visual Studio 2022 Community" a ako šablónu som si vybral jednoduchú "Console.Application".
Pri prístupe k zadaniu som najskôr preskúmal výstupy zo súboru "ESET_Parser attachment.log". Po zoznámení sa s formátom týchto výstupov som začal pracovať na svojej implementácii programu.
Postupoval som tak, že ak súbor obsahoval riadok s "threat="is OK"", považoval som ho za neinfikovaný. Ak však obsahoval napríklad "threat="a variant of Win32/TrojanDownloader.Delf.PWL trojan"", považoval som ho za infikovaný a vypísal som ho na konzolu.
</p>
<p>
  🥉 Na začiatku program požiada používateľa, aby zadal cestu k súboru, ktorý sa má parsovať. Ak používateľ zadá neplatnú cestu, program nebude schopný správne fungovať. Po zadání správnej cesty k parsovanému súboru program extrahuje záznamy, ktoré sú označené ako infikované, a zobrazí ich v navrhnutom formáte. Tento formát pozostáva z niekoľkých častí. Prvá časť je "File path:", 
  ktorá obsahuje cestu k infikovanému súboru. Nasleduje "Threat:", kde je uvedená hrozba alebo vírus obsiahnutý v danom súbore. Ak je súbor archivovaný, program vypíše informáciu o archíve 
  (napríklad: Archive: NSIS). Ak súbor obsahuje aj "packer" (čo som si vysvetlil ako súbor za archívom), program ho taktiež zobrazí. Ak archív alebo packer sa nenachádzajú v danom súbore program ich nevypíše na konzolu. 
 Na záver programu sa zobrazia počty súborov v celom súbore, počet infikovaných súborov a počet neinfikovaných súborov.
</p>

<h1 align="center"> Ukážka fungovania kódu: </h1>
