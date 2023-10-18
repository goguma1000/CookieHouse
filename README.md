# Cookie House
2인 멀티플레이 VR 방탈출 게임으로 네트워크 엔진으로 Photon Fusion을 사용하였으며, VR기능은 XR Interaction toolkit을 사용함.

코드 경로: [Scripts](https://github.com/goguma1000/CookieHouse/tree/main/CookieHouse/Assets/Scripts)

**트레일러:** [Link](https://youtu.be/qXvOeSiNOpk) (0:46)   
**플레이 영상:** [Link](https://youtu.be/CEsWqRJp4Zk) (06:14) 
## 목차
**[네트워크](#네트워크)**  
   &nbsp; **- [매치 메이킹](#매치-메이킹)**    
   &nbsp; **- [매칭 룸](#매칭-룸)**   

**[퍼즐 워크플로우](#퍼즐-워크플로우)**  
   &nbsp; **- [그림, 뼈 완성 퍼즐](#그림-뼈-완성-퍼즐)**  
   &nbsp; **- [그림자 퍼즐](#그림자-퍼즐)**  
   &nbsp; **- [책장 정리 퍼증](#책장-정리-퍼즐)**  
   &nbsp; **- [아궁이 퍼즐](#아궁이-퍼즐)**  
   &nbsp; **- [체스 퍼즐](#체스-퍼즐)**  

---
### 네트워크
#### 매치 메이킹
 - **세션 생성**  
    **Flow Chart**  
     ![그림1](https://github.com/goguma1000/CookieHouse/assets/102130574/a1733a57-9486-45c1-8789-676c3bf4ff26)
  

     세션 생성은 유저가 Create 버튼을 누르면  OnCreateSession 함수를 호출하여   
     NetworkManager에서 Connect 함수를 통해 Network Runner를 생성하고  
     Network Runner의 StartGame 함수를 통해 네트워크 모드, 최대 인원수 등 방의 속성에 대해 설정한 후 세션 생성한다.</br>

    **관련 코드 링크 :**  
    [NetworkManager.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/NetworkManager.cs)   
    [NewSessionTab.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Session/NewSessionTab.cs)  
    </br>

 - **세션 리스트 업데이트 및 세션 참가**  
    **Flow Chart**  
     ![그림2](https://github.com/goguma1000/CookieHouse/assets/102130574/2377294b-5182-4eb2-9fa0-cf905f781c88)  
     Session List가 업데이트되면 OnSessionListUpdate 함수가 호출되며 유저가 로비에 접속할 때 등록했던 Delegate를 호출한다.  
     ~~~cs
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            SetConnectionStatus(ConnectionStatus.InLobby);
            onSessionListUpdated?.Invoke(sessionList);
            ...
        }
     ~~~ 
     
     UpdateSeessionList 함수를 통해  세션이 Session List에 등록된다.  
     ![그림3](https://github.com/goguma1000/CookieHouse/assets/102130574/4a7a4e8b-193b-4299-a9fd-a89280ddf2a9)     
     이때 Join 버튼에 JoinSession 함수가 Delegate로 등록이 되며,   
     유저는 Join 버튼을 눌러 세션에 참가할 수 있다.  

     **관련 코드 링크 :**  
    [SessionListPanel.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Session/SessionListPanel.cs)  
    [SessionListItem.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Session/SessionListItem.cs)  
    [NetworkManager.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/NetworkManager.cs)   
      
    </br>

 #### 매칭 룸
 ![그림5](https://github.com/goguma1000/CookieHouse/assets/102130574/22f6551c-8c6e-483e-8a31-5dd2cc641146)  
 매칭 룸은 캐릭터 선택 버튼, 유저 정보, 시작 버튼으로 구성되어 있다. 
- **유저 정보 업데이트**  
   **Flow Chart**  
   ![그림4](https://github.com/goguma1000/CookieHouse/assets/102130574/6b562b67-f442-4b7a-853d-b242f3d389cd)  
   세션에 참가한 유저 수가 변하면 Network manager에서 세션에 참가한 유저 정보를 받아와 업데이트한다.  
   만약 어떤 유저가 나갔다면 그 유저가 선택했던 캐릭터 버튼을 초기화한다.  

    **관련 코드 링크 :**  
    [LobbyManager.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/LobbyManager.cs)   
    [NetworkButton.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/NetworkButton.cs)    
    </br>
- **캐릭터 선택 동기화**  
    **Flow Chart**  
    ![그림6](https://github.com/goguma1000/CookieHouse/assets/102130574/46a5d447-b2b8-4fdf-84af-ac5e40fbfc3a)  
    유저가 캐릭터를 선택하면 해당 버튼에 유저 정보를 저장하고, 선택한 유저의 이름을 띄운다.  
    다시 한번 동일한 캐릭터를 선택하면 캐릭터 선택이 해제된다.  
    ![그림7](https://github.com/goguma1000/CookieHouse/assets/102130574/cdfcd7f1-bc48-4311-b197-b5e1b0b22733)   
    캐릭터 버튼은 두 유저가 공유하고 있으므로 Photon에서 제공하는 Networked 프로퍼티로 변수를 선언하여 버튼의 선택정보를 동기화한다.  
    Player 객체는 두 유저가 공유하고 있지 않으므로 유저의 선택정보를 RPC로 동기화한다.  

    **관련 코드 링크 :**  
    [NetworkButton.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/NetworkButton.cs)    
    [Player.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Player/Player.cs)  
    </br>
    
- **게임 시작**  
    **Flow Chart**  
    ![그림8](https://github.com/goguma1000/CookieHouse/assets/102130574/cb264dc9-c130-4e4e-beb8-7c7c3f3ae8ce)    
    Update 함수에서 두 유저가 모두 캐릭터를 선택했는지, 방에 참여한 유저가 2명인지 확인한다.  
    두 유저가 모두 캐릭터를 선택하면 두 유저가 같은 캐릭터를 선택했는지 확인하고,  
    같은 캐릭터를 선택했다면 두 유저의 선택 정보를 초기화한다.  
    다른 캐릭터를 선택했다면 start 버튼이 활성화되어 세션의 주인이 게임을 시작할 수 있다.

    **관련 코드 링크 :**  
    [LobbyManager.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/LobbyManager.cs)   
    [NetworkManager.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/NetworkManager.cs)    
    </br>

- **Networked Property**  
   ~~~cs
   // Networked Property 예시
    [Networked]
    public string playerName { get; set; }

    [Networked(OnChanged = nameof(OnSelected))]
    public int Owner { get; set; }
   ~~~  
   Photon에서 제공하는 프로퍼티로, 해당 프로퍼티로 선언한 변수의 값이 변경되면  
   OnChanged에 등록된 함수를 호출하여 동기화 할 수 있다.  
   객체에 대한 State Autority를 가진 유저가 값을 변경했을 때만 OnChanged Callback이 호출되므로  
   해당 변수의 값을 바꾸기 전에 객체의 State AUthority를 얻어야 한다.  

---  

### 퍼즐 워크플로우
본 게임에는 **그림, 뼈 완성 퍼즐**, **그림자 퍼즐**, **책장 정리 퍼즐**, **아궁이 퍼즐** 그리고 **체스 퍼즐**이 있다.  

#### **그림, 뼈 완성 퍼즐**  

  ![그림9](https://github.com/goguma1000/CookieHouse/assets/102130574/71928a59-88d5-4227-89b4-07cdca96f88c) | ![그림10](https://github.com/goguma1000/CookieHouse/assets/102130574/f93c8f0a-d2e7-4215-9304-2f2fea63a90b)  
  ---|---|   

  - **Flow Chart**   
  ![그림11](https://github.com/goguma1000/CookieHouse/assets/102130574/55c36472-4eef-466b-8818-e5d2f8d16ab9)  
  퍼즐 조각과 퍼즐이 놓일 위치 사이의 거리가 일정 거리만큼 가까워지면 퍼즐 조각이 맞춰진다.  
  모든 퍼즐조각이 맞춰지면 다음 퍼즐이 활성화된다.  

  **관련 코드 링크 :**  
    [Piece.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/Piece.cs)    
    [PuzzleBoard.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/PuzzleBoard.cs)   
    </br>  

#### **그림자 퍼즐**  
![그림12](https://github.com/goguma1000/CookieHouse/assets/102130574/f826dd64-fdf8-4327-8e2c-820c024c816c)  
- **Flow Chart**  
  ![그림13](https://github.com/goguma1000/CookieHouse/assets/102130574/e3344815-ddcc-4aad-9624-0f53591a9d32)  
  Up 벡터와 Right 벡터로 각각 내적했을 때 임계 값 이상이 되면 퍼즐이 풀리고 방문이 열린다. 

  **관련 코드 링크 :**  
    [ShadowMatch.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/ShadowMatch.cs)    
    </br>

#### **책장 정리 퍼즐**  
![그림14](https://github.com/goguma1000/CookieHouse/assets/102130574/f5d99e54-d56d-47b1-b6c3-64dd13ed6824)  
- **Flow Chart**  
  ![그림15](https://github.com/goguma1000/CookieHouse/assets/102130574/6ad43938-c9b4-49ab-8b78-036d271fa769)  
  OnTriggerEnter 이벤트가 발생했을 때 정답 책장에 두었는 지 확인하고 오브젝트간 위치를 조정한다.  
  각 오브젝트들이 정답 책장에 적절히 들어가면 퍼즐이 풀리고  
  다음 이벤트에 사용될 아이템을 활성화한다.  

  **관련 코드 링크 :**  
    [Book.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/Book.cs)  
    [BookShelf.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/BookShelf.cs) 
    </br>  

#### **아궁이 퍼즐**  
![그림16](https://github.com/goguma1000/CookieHouse/assets/102130574/55bf4b36-65c4-461d-b6d2-5c3dd2521f9a) 
- **Flow Chart**  
  ![그림17](https://github.com/goguma1000/CookieHouse/assets/102130574/fa93f1e6-1819-4aeb-8e9d-d934c2206a68)  
  유저가 아궁이의 손잡이를 잡으면 Simple interactable 컴포넌트에 의해 Grab intreactable 컴포넌트가 활성화되고  
  나머지 한 손으로 손잡이를 잡으면 아궁이 문을 열 수 있다.  
  만약 두 손 중 한 손이라도 놓으면 아궁이 문이 다시 닫힌다.  
  열쇠 안에 초를 넣으면 퍼즐이 풀리며 다음 이벤트에 사용될 아이템을 활성화한다.
   
  **관련 코드 링크 :**  
    [FirePlaceDoor.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/FirePlaceDoor.cs)  
    [Fire.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/Fire.cs)  
    </br>

#### **체스 퍼즐**
![그림18](https://github.com/goguma1000/CookieHouse/assets/102130574/092831e9-8ba9-4adb-897a-eb428e66b4d9)  
- **Flow Chart**  
  ![그림19](https://github.com/goguma1000/CookieHouse/assets/102130574/27a08f56-4462-41ed-99b7-56edfa214493)  
  체스 말을 정답 위치에 놓으면 퍼즐이 풀리고, 틀리면 체스 말이 원위치로 돌아간다.  
  만약 유저가 체스 말을 잡은 지 1분 이상이 되면 체스판에 힌트가 활성화된다. 

  **관련 코드 링크 :**  
    [Chess.cs](https://github.com/goguma1000/CookieHouse/blob/main/CookieHouse/Assets/Scripts/Puzzle/Chess.cs) 
    </br>




     

    
    
