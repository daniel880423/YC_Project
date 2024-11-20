# ISSUE’S GENERAL INFORMATION
| Issue Date |部門 / Owner |Customer |CP / FT |Device Issue |平台 |異常判定 |Keywords |Application |
| ---------- |---------- |---------- |---------- |---------- |---------- |---------- |---------- |---------- |
| 2024/01/16 |7210/楊勝雄 |PHISON |FT |PS5017 BGA169 Flash RDT TYP測試項量產All Site Fail ISSUE |E320BD-CH |程式 |MOS Relay, All Site Fail |SATA Flash Controller |
# ANALYSIS AND SOLUTION
## 1. Current State
* 前言:
   * 此產品在特定機台量產出現SBin63 FT1_FT_RDT不良率過高問題，且Fail當下幾乎為All Site Fail。
* 問題描述:
      1. PS5017 BGA169 Release機台為CH003/CH008，使用V15程式在CH008有偶發RDT All Site Fail Issue，V15程式在CH003則無此問題。
      2. V15程式量產BGA169貨批TO10079AA1:
         * &rarr; 正測Yield: 85.21%(22153/25999) 。
         * &rarr; Major Fail: SBin63 FT1_FT_RDT - 6.58%(1712/25999) 。
         * &rarr; SBin63 Recovery Rate - 98.13%(1680/1712) 。
## 2. PotentialRoot Cause  Analysis
* 機台/配件確認:
   1. CH008工程驗證皆為All Site Pass，初步判斷機台/配件正常，無法還原RDT All Site Fail狀況，請設備PM機台，量產仍然會出現RDT All Site Fail(非真因) 。

   ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.002.png)

* UI確認:
      2. 量產UI版本為316_32，提出先Fail的Site會把其他Site URC 一起off的疑慮，請軟體部幫忙實驗，TD也同步使用公版DUT Board焊接COTO2211於實驗室BDC機台進行實驗。
      1. 硬體配置:
         * &rarr; 使用兩顆COTO2211 Relay切換DPS to I/O， DPS送電給I/O，以Compare Z做為Pass/Fail依據。

         ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.003.png)

      2. 程式設定:
         * &rarr; 仿Flash RDT TYP測試項建立深度接近的Pattern(3.9sec)分段跑， DPS與URC Channel皆為獨立，將Site A compare Fail再去執行2 Site Flow Run。

         ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.004.png)

      3. TD實驗結論:
         * &rarr; 使用URC沒有勾選Multi-site時，最後才同時動作 以跑Flow方式驗證，當Site A提早Fail時，Site A URC off，此時Site B繼續跑Pattern，直到Site B跑完Pass，Site B URC off，故沒有site A提早Fail後，把Site B的URC一起off問題(非真因) 。
         * &rarr; URC有勾選Multi-site時，最後才同時動作 以跑Flow方式驗證，確認提早Fail的Site A會關電，但URC不會先off，等到Site B跑完後，才一起將2 Site URC一起off(非真因) 。
      4. 軟體部/硬體部工程師實驗結論:
         * &rarr; URC on/off在316_32 UI硬體與軟體動作一致。
         * &rarr; 實驗2 Site Flow Run， Fail Site將URC off時，不會同時把其他的Site URC off掉，URC動作正常。
* 程式確認:
            3. L\B早期規劃一組3.3V驅動4 Site MOS Relay(5 pcs/1 Site)，程式電流檔位500mA(Clamp=±100mA)，若Contact不好導致Loading變大，100mA電流不足以讓4 Site MOS Relay正常做動(真因) 。

            ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.005.png)

## 3. Root Cause Analysis
* 真因確認:
   1. 上機驗證RDT Fail IC皆為Pass(誤宰)，RDT All Site Fail真因分析應為Index Arm Contact偏差問題影響負載電流，此時3.3V /Scale 500mA(Clamp=±100mA)給All Site MOS Relay使用，電流會不夠導致切換異常，故將Clamp放寬。

   ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.006.png)

   2. 於CH008僅有一次還原All Site RDT連續Fail問題，將4 Site MOS Relay共用的Power 3.3V/Scale 500mA電流Clamp放大至±500mA，3 Site Loop 10次Pass確認有改善。

   ![圖片](images/PHISON_PS5017_FT_BGA169_Flash_RDT_TYP.007.png)

## 4. Root CauseSummary
1. 驗證困難點為無法有效還原Flash RDT TYP All Site Fail環境，僅在其中一次實驗中成功還原問題，並且修改程式Fail to Pass，V16程式已加入MOS Relay Power 3.3V/ Scale 500mA(Clamp=±500mA) 。
2. Flash RDT TYP測試項因為Contact不佳導致負載電流變大，而原本MOS Relay Power 3.3V的電流Clamp=±100mA不夠使用，故放寬至Clamp=±500mA讓電流可以供MOS Relay正常運作。
3. Flash RDT TYP測試項量產狀況:
   1. CH008:
      * &rarr; V15(TO10079AA1)正測不良率6.58%(1712/25999) 。
      * &rarr; V16(TO3004BAA3)正測不良率1.5%(439/29213) 。
   2. CH003:
      * &rarr; V15(TNA01SZAA2)正測不良率0.02%(7/41629) 。
      * &rarr; V16(TO30070AA1)正測不良率0.02%(6/37512) 。
## 5. Solution
1. 當產品有一推多組MOS Relay時，需要將供應電流開大，以防止當Contact不佳時，負載電流變化導致MOS Relay切換異常。
