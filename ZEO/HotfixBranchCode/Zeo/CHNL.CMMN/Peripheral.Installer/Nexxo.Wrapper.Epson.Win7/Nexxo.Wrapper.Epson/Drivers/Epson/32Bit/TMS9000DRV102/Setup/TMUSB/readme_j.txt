
                        TMUSB device driver version 6.10a

                                                                   2012/03/15
                                                     セイコーエプソン株式会社

１．はじめに
------------
    TMUSB は EPSON TM/EU/BAシリーズのデバイスと通信するための USB デバイスド
  ライバです。
    デバイスドライバのインストールはデバイスウィザードに従って行いますが、本
  ユーティリティにて TMUSB デバイスドライバのサイレントインストールが行えま
  す。
  
２．TMUSB 動作環境
------------------
2.1 動作 OS
  ・ Microsoft(R) Windows(R) 2000 Professional SP4
  ・ Microsoft(R) Windows(R) XP Professional SP3 32bit版
  ・ Microsoft(R) Windows(R) XP Professional SP2 64bit版
  ・ Microsoft(R) Windows Server(R) 2003 R2 SP2 32bit版
  ・ Microsoft(R) Windows Server(R) 2003 R2 SP2 64bit版
  ・ Microsoft(R) Windows Vista(R) SP2 32bit版
  ・ Microsoft(R) Windows Vista(R) SP2 64bit版
  ・ Microsoft(R) Windows Server(R) 2008 SP2 32bit版
  ・ Microsoft(R) Windows Server(R) 2008 SP2 64bit版
  ・ Microsoft(R) Windows Server(R) 2008 R2 SP1 64bit Version
  ・ Microsoft(R) Windows 7(R) SP1 32bit Version
  ・ Microsoft(R) Windows 7(R) SP1 64bit Version

  ※ 上記OSをベースとしたEmbedded系OSにも対応しています。
  ※ Microsoft(R) Windows2000は、インテル(R) HTテクノロジー に対応していません
     のでBIOS 設定等で インテル(R) HTテクノロジー を OFF にしてお使いください。
  ※ Microsoft, Windows, Windows Serverは、米国 Microsoft Corporation の、
     米国、日本およびその他の国における登録商標または商標です。
    インテルは、アメリカ合衆国およびその他の国におけるIntel Corporationまたは
    その子会社の商標または登録商標です。
  ※ 引用した会社名・商品名は、各社の商標または登録商標です。

2.2 動作 USB ホストコントローラ
  ・ インテル(R) 製 チップセット組み込みの USB ホストコントローラ
  ・ NEC 製 EHCI USB ホストコントローラ

  ※ NEC 製 USB 1.1 用 の OHCI ホストコントローラでの動作は保証していません。

2.3 動作 USB ドライバスタック
  Microsoft(R) 製(OS標準)のドライバスタックをお使いください。
  また、Microsoft(R) 製の USB ドライバスタックはできるだけ最新のものをご使用く
  ださい。サードパーティ製の USB ドライバスタックでの動作は保証していません。
  
  ※ USB 2.0 High Speed 接続時には Microsoft(R)製 USB ドライバスタック
    usbehci.sys のファイルバージョンをご確認ください。
  
     Microsoft(R) Windows(R) 2000 Professionalの場合
        5.00.2195.6709 以降
     Microsoft(R) Windows(R) XP   Professionalの場合
        5.1.2600.1243 以降
     Microsoft(R) Windows Server(R) 2003 の場合
        5.2.3790.1830 以降
     Microsoft(R) Windows Vista(R) の場合
        6.0.6000.16386 以降
     Microsoft(R) Windows Server(R) 2008の場合
        初回リリース以降
     Microsoft(R) Windows 7(R) の場合
        初回リリース以降

  ※ USB ドライバスタック最新版への更新方法
     1) インターネットに接続します
     2) コントロールパネルのシステムをダブルクリックします
     3) 「ハードウェア」タブを選択します
     4) 「デバイスマネージャ」ボタンを押します
     5) USB コントローラを選択します
     6) 「xxx USB Enhanced Host Controller」をダブルクリックします
     7) 「ドライバ」タブの「ドライバの更新」より最新版へ更新できます
  
  ※ ドライバスタック usbehci.sys のファイルバージョンの確認方法
    1-1) デバイスマネージャーから「xxx USB Enhanced Host Controller」をダブル
         クリックします
    1-2) 「ドライバ」タブの「ドライバの詳細」ボタンを押す
  
    または
  
    2-1) ウィンドウシステムディレクトリの system32\drivers\usbehci.sys ファイ
         ルを右クリック
    2-2) プロパティを選択し、バージョン情報で確認

2.4 動作 USB HUB
  ・ USB 1.1 規格のHUB(Full Speed 対応)
  ・ USB 2.0 規格のHUB(High Speed 対応)
  
  ※ USB 1.0 規格の USB HUBの動作は保証していません。
  ※ USB 2.0 High Speed に対応した PC に、USB 2.0 HUB経由でデバイスを接続す
     ると、USB 2.0 High Speed に対応していないデバイスでも、USB 2.0 High Spe
     ed 接続となりますので、USB 2.0 High Speed 接続時の使用条件に従ってくだ
     さい。

2.5 動作デバイス
  ・EPSON TM/EU/BAシリーズのデバイス
  
2.6 動作 USB 接続
  ・最大 USB ケーブル長 5 m
  ・最大 HUB 数         5 段
  ※ USB 2.0 規格に適合している USB ケーブル、USB HUBを使用すること
  
2.7 最大接続台数
  下記の台数までを動作保証接続台数とします。
  ・OPOS                      : 最大 10 台
  ・APD                       : 最大  8 台
  ・その他のドライバ、ツール  : 最大  8 台
  
2.8 省電力が有効な組み合わせ
  デバイスのUSB 省電力機能が有効になるのは以下の組合せになります。
  
    OS       : Microsoft Windows XP
    Device   : TM-T88IV 10.01 ESC/POS 以降
               TM-T70, TM-S1000
               TM-T88V, TM-T20, TM-H6000IV, BA-T500II
               TM-S9000, TM-S2000
    
  ※ USB 2.0 HUB 経由でデバイスを接続した場合は、省電力機能は有効になりませ
     んので、直接 PC と接続してください。
  ※ USB 1.0/1.1 HUB 経由でデバイスを接続した場合、OSの制限でUSB HUBの種類に
     より正常動作しない場合がありますので問題がある場合は直接 PC と接続してください。
　
　※ Windows Vista/2008の省電力機能は
     デフォルト無効になっています。
     有効にしたい場合は、-p1オプションを指定してインストールしてください。
     その場合、デバイスマネージャーから、USBルートハブの電力管理の設定の
 　　[電力の節約のために、コンピュータでこのデバイスの電源をオフにできるようにする]
     を必ず有効にしてください。

３．フォルダの説明
------------------
  以下のようなフォルダ・ファイル構成となっています。

   + TMUSB500a
     + SETUP.exe
     + TMUSBXP ..................以下のOS用のTMUSBが入っています
                                  Microsoft(R) Windows(R) 2000
                                  Microsoft(R) Windows(R) XP 32Bit版
                                  Microsoft(R) Windows Server(R) 2003 R2 32Bit版
                                  Microsoft(R) Windows Vista(R) 32Bit版
                                  Microsoft(R) Windows Server(R) 2008 32Bit版
                                  Microsoft(R) Windows 7(R) 32Bit版

                                  
     + TMUSB64 ..................以下のOS用のTMUSBが入っています
                                  Microsoft(R) Windows(R) XP 64Bit版
                                  Microsoft(R) Windows Server(R) 2003 R2 64Bit版
                                  Microsoft(R) Windows Vista(R) 64Bit版
                                  Microsoft(R) Windows Server(R) 2008 64Bit版
                                  Microsoft(R) Windows Server(R) 2008 R2 64Bit版
                                  Microsoft(R) Windows 7(R) 64Bit版
     + ReadMeJ.txt
     + ReadMeE.txt

４．デバイスドライバーのインストール方法
----------------------------------------

  Windows(R) 2000/ Windows(R) XP/ Windows Server(R) 2003にインストール/アッ
  プデートを行なう場合には、インストル/アップデート作業を行う前に、コンピュー
  タの管理者 もしくは コンピュータの管理者の権限を持つユーザー名でログオンされ
  ていることをご確認ください。
  Windows Vista(R) /Windows Server(R) 2008/Windows 7(R)にインストールする場合は
  SETUP.exe を管理者権限に昇格して実行する必要があります。

4.1 ウィザードによる(通常)ドライバーインストール手順

  1) USB ケーブルで接続されたデバイスの電源を投入します
  2) 「新しいハードウェアの検出ウィザード」が起動します
  3) 「一覧または特定の場所からインストールする(詳細)」を選択します
  4) 「次の場所で最適なドライバを検出する」を選択し、ご使用のＯＳに適したフ
     ォルダの場所を指定します
  5) 「完了」ボタンでインストールを終了します

4.2 ドライバーのアップデート手順

  1) USB ケーブルで接続されたすべてのデバイスの電源をOFFにします
  2) もし「新しいハードウェアの検出ウィザード」が表示している場合は
      ウィザードをキャンセルします
  3) SETUP プログラムを実行します
  4) デバイスの電源を投入します

4.3 サイレントインストール手順

  ※ サイレントインストールを実現するにはデバイスをＰＣに接続する前にセッ
     トアッププログラムを実行する必要があります。

  1) USB ケーブルで接続されたすべてのデバイスの電源をOFFにします
  2) 「新しいハードウェアの検出ウィザード」が表示している場合は
      ウィザードをキャンセルします
  3) SETUP プログラムを -s2オプションをつけて管理者権限で実行します
    
    使用許諾契約が表示されます。内容をご確認頂き同意される場合は、
    「使用許諾契約に同意する」を選択し、インストールボタンを押してください。
    
    SETUP プログラムが ドライバ情報ファイル(INF)とドライバファイル(SYS)をシ
    ステムにコピーします。

    セットアッププログラムはインストール結果を 環境変数 ERRORLEVEL に返します
      0    : 成功した
      2    : インストールファイルが見つからなかった
      1151 : サポートされていない OS で実行した
      1223 : ユーザがインストールをキャンセルした
      3010 : インストールは成功したが、システムのリブートが必要

  4) デバイスの電源をONしたときにハードウェアの追加がサイレントで行なわれ
     ます

4.4．起動時オプション

    -s2 サイレントインストール
        既にインストールされているドライバが新しければインストールしない
    -p1 デバイス省電力機能を有効にします
    -p2 デバイス省電力機能を無効にします

4.5. TM-C100のインストール時の注意

    TM-C100 本体をホストPCに接続する前に必ず、TMUSB の SETUP プログラムを
  実行、または、TM-C100 用のプリンタドライバを先にインストールしてください。
    もし、TM-C100 を PC に接続し電源を入れてもデバイスマネージャに
 「EPSON USB Controller for TM/BA/EU Series」が表示されない場合は、以下
  の手順で復帰させてください。

    1) TM-C100 を PC に接続し電源を入れます
    2) デバイスマネージャに「USB 印刷 Support」が表示されている場合
       「USB Printing Support」を右クリックし「削除」を選択します
    3) TM-C100 の電源を入れ直します
    4) デバイスマネージャに「EPSON USB Controller for TM/BA/EU Series」
       が表示されるか確認します

５．制限事項
------------
  ・ Windows(R) 2000/Windows(R) XP/Windows Server(R) 2003 のインストールは
     必ず管理者権限で行ってください
  ・ Windows Vista(R)/Windows Server(R) 2008/Windows 7(R)にインストールする場合
     は、SETUP.exe を管理者権限に昇格して実行する必要があります。
  ・ TMUSBドライバを利用しているアプリケーションを終了してからセット
     アッププログラムを実行してください
  ・ プリンタに印字中に、電源のOFF/ONや、ケーブルの抜き差しはしないでください
  ・ デバイスの電源をOFFにした後、TMUSBドライバがアンインストールされる時間
     (約５秒)待ってからデバイスの電源をONしてください
  ・ デバイスドライバがロードされている状態（デバイスの電源がONの状態）で
     セットアッププログラムを実行した場合には、メモリ中のデバイスドライバの更
     新が行われません。デバイスの電源OFF/ONまたはシステムの再起動を行って
     ください
  ・ Windows(R) 2000 では 管理者権限でログインしていないとドライバ署名ありと
     表示されません
  ・ サイレントインストーラをご使用になる場合には、インターネットエクスプローラ
     Version 4.0 以降が必要です
  ・ デバイスの動作中は、OSの制限でコンピュータをスタンバイや休止状態へ移行で
     きない場合があります
     コンピュータをスタンバイ/休止状態へ移行中にデバイスの電源を切る、もしくは
     USBのケーブルを抜くと、OSの制限でシステムが不安定になることがあります。
     印刷アプリケーションを終了、もしくはデバイスの電源をOFFにしてからコンピュ
     ータをスタンバイ/休止状態にしてください

６．TMUSB ドライバ履歴
----------------------
  ・ 2012/03/15 Version 6.10a Windows 7での強制スタンバイ問題を対処
  ・ 2012/01/27 Version 6.00a 省電力関連のブルースクリーン問題を対応
  ・ 2009/11/25 Version 5.10a スキャナー画像受信に失敗する問題を対応
  ・ 2009/09/17 Version 5.00a Windows 7(R)対応、非同期通信対応
  ・ 2009/04/06 Version 4.00c XP 64bit署名 追加取得
  ・ 2009/01/13 Version 4.00b 64bit環境でのインストール不具合を対処
  ・ 2008/10/31 Version 4.00a TMCOMUSBドライバと他のドライバとの共存対応
  ・ 2008/05/20 Version 3.20d TMUSBのインストーラのメモリアクセス違反不具合を対処
  ・ 2008/01/28 Version 3.20c TMUSBのインストーラの不具合を対処
  ・ 2007/10/29 Version 3.20b Windows(R) 2000のWHQLドライバ署名の不具合を対処
  ・ 2007/09/26 Version 3.10b TM-S1000 CDパッケージ用
  ・ 2007/09/07 Version 3.10a TM-S1000対応,Windows Vista(R) 対応 
  ・ 2007/01/19 Version 2.20a スタンバイに移行できない不具合対応
  ・ 2005/12/16 Version 2.10a TM-T88IV 省電力対応
  ・ 2005/10/28 Version 2.06a TMCOMUSB対応(ドライバ署名なし)
  ・ 2004/10/15 Version 2.00a J9000/TMCOMUSB 対応版
  ・ 2004/01/15 Version 1.91a High Speed HUB 対応版
  ・ 2003/09/25 Version 1.81a Windows(R) 2000 SP4 正式対応版

--- EOF ---
