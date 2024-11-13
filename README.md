|   ISSUE’S GENERAL INFORMATION   |  Describe  |
|  ---------  |  ---------  |
|  Issue Date  |  2024/03/06  |
|  部門 / Owner  |  7290 / 林宥廷  |
|  Customer  |  SITRONIX  |
|  CP / FT  |  CP  |
|  Device Issue  |  SITRONIX-AA003 影像補點縮時  |
|  平台  |  I6000B  |
|  異常判定  |  機台硬體、方法  |
|  Keywords  |  OTP、Trim影像補點、TTR  |
|  Application  |  Light Sensor  |

|   ANALYSIS AND SOLUTION   |  Describe  |
|  ---------  |  ---------  |
|  1. Current State  |  SITRONIX-AA003影像補點(BPC) Trim  1. P/C設計共16 Site，此產品每個Site至多寫入64組補點資料，每組由3筆8bit資料組成。2. 未使用到空間不可燒入且Trim 0亦不可重複燒入，故有誤燒風險須避免。3. 每個Site補點資料數量不相同，將導致每個SiteTrim Pattern執行長度不同。4. 在目前E320機台硬體架構下僅能使用Series run 對各Site分別執行不同長度Pattern，這將花費較多時間，並不符合客戶成本。  |
|  2. PotentialRoot Cause  Analysis  |  1. Normal Trim Pattern(Series Run)：1.將燒入值十進制轉變為8bit二進制值。2. 於Pattern SDA相對應行數動態改Pattern，帶入二進制值。3. 使All site執行Pattern 0到最後一行。(附件一)4. MAX 64*3*8bit資料。 ![圖片](images/SITRONIX-AA003_影像補點縮時.002.png) 2. 未使用到空間不燒入(Series Run)：  1. 未使用到空間不可燒入，每個site Pattern結束行數不同，需使用CBASE Series run 執行Pattern。(附件二)![圖片](images/SITRONIX-AA003_影像補點縮時.003.png)3. 將未使用到空間SCL設為0 (Parallel Run)：  1. 動態改Pattern將未使用空間的SCL改為0，執行到此段時，因SCL沒有持續送訊號，則使IC停止燒入(減少誤燒機會)。  2. 使All site 執行Pattern 0到最後一行。(附件三)![圖片](images/SITRONIX-AA003_影像補點縮時.004.png)  |
|  3. Root Cause Analysis  |  Environment & Product analysis:  1. 使用SCL為0方式進行Subtest設定。(附件四)  2. CBASE程式內容撰寫方式，使用動態改Pattern將未使用空間的SCL改0，以減少測試時間與誤燒機會。(附件五)  |
|  4. Root CauseSummary  |  1. 依不同Trim條件模擬，在三種測試方式的測試時間整理如下表與下圖所示。![圖片](images/SITRONIX-AA003_影像補點縮時.005.png)![圖片](images/SITRONIX-AA003_影像補點縮時.006.png)  |
|  5. Solution  |  1. 由圖表可得知將未使用空間的SCL設為0方式，可有效降低BPC Trim測試項約62%~74%的測試時間，並且也可避免誤燒風險，故此Case採用將未使用空間的SCL設為0並執行全部Pattern方式進行縮時。  2. 未來可持續優化縮時，將此次與前一次動態改Pattern的內容進行比對，只需修改相同的Ram但資料不同的位置，即可再更進一步的縮時。  |
|  6. Attachment  |  附件一![圖片](images/SITRONIX-AA003_影像補點縮時.007.png)附件二EX : 如下圖Site 0執行至11662行，Site1執行至2446行。![圖片](images/SITRONIX-AA003_影像補點縮時.008.png) 附件三![圖片](images/SITRONIX-AA003_影像補點縮時.009.png)附件四![圖片](images/SITRONIX-AA003_影像補點縮時.010.png)附件五![圖片](images/SITRONIX-AA003_影像補點縮時.011.png)  |

