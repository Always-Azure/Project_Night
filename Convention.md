# Project Convention

## [파일명]
+ 파일 명은 모두 대문자로 시작해야하며, Pascal Cae로 작성해야 한다.
+ _(언더바)는 사용하지 말 것.

## [Asset]
+ Texture 및 Merarial 은 모델링 명과 동일하게 할 것.
+ UV mapping 데이터는 이름_UV라고 할 것.

## [코딩관련]
+ 함수, 클래스, 이벤트 등은 PascalCase로 작성 (대문자로 시작해서 단어 연결 시, 대문자로 시작. ex) PascalCase)
+ 배열 선언은 tempArr

+ **<변수>**
  - camelCase로 작성.(소문자로 시작해서 단어 연결시, 대문자로 시작. ex) camelCase)
  - private변수는 _로 시작해서 구분할 것.
  - 변수명이 길어도 생략하지말고 다 작성해야할 것.
  - 같은 접근지시자 내부에서 관련있는 것끼리 그룹화시킬 것.
    ````c
    ex)
    public int item1;
    public int item2;
    public int random1;
    public int random2;
    ````
  - 중복된 이름의 변수를 사용할 시, 1부터 넘버링 할 것.
    ````c
    ex) item1, item2;
    ````
  - 데이터 크기 순서대로 선언할 것.
  - static -> const -> 일반 순으로 선언할 것.
    ````c
    ex)
    public static int bb;
    public static double dd;
    public const char aa;
    public const float cc;
    public char a;
    public int b;
    public float c;
    public double d;
    ````
+ **<함수>**
  - PascalCase를 사용할 것
  - bool 반환 함수는 is로 시작할 것. ex) bool IsAbcd();
  - 한 줄에 한 구문씩 작성해야한다.
  - 반복문에서 실행문이 한 줄이여도 block을 작성해야한다.
  - 함수 내부에서 선언할 변수들은 처음에 선언할 것.
  - 예외처리는 변수 선언 다음에 할 것!
  - 비슷한 유형의 구문들은 그룹화해서 사용하자.
    ````c
    ex)
    a = b;
    c = a + 2;

    for(......

    a = c;
    c += b;

    for(......
    ````
   - 전체 예시
    ````c
    bool IsAsdf(int a, int b, bool isValid){
       // 선언부
       int c;
       int item;

       // 진입부
       if(!isValid){
          return false;
       }

       // 할당부(할당은 중간에 될 수 도 있다.)
       c = a;
       item = b;

       // 함수 내용
       for(..........
       ...
    }
    ````
 + **<클래스>**
   - 클래스 이름은 반드시 대문자로 시작해야한다.
   - 클래스 내 변수 및 함수들은 접근 지정자를 반드시 작성해야한다. ex) public, protected, private
   - 접근 지정자 순서는 public, protected, private 이다.
   - 멤버 변수는 앞에 m을 붙일 것.
   - getter, setter 함수는 class영역 마지막에 작성할 것.
   - 멤버 함수는 선언 된 순서대로 작성할 것.
   - 멤버 변수는 접근자끼리 작성해두고 다른 접근자는 newline으로 구분할 것
    ````c
    ex)
    public int a;
    public int b;

    private int c;
    private int d;

    protected int e;
    protected int f;
    ````


 + 함수의 인자를 작성할 때, 한 칸 띄워줘야한다.
     ````c
    ex) transform.Translate(1f, 2f, 3f);
    ````
 + 흐름 제어문(연산자 등)에 띄어쓰기 해줘야 한다.
     ````c
    ex) while(x == y)
        a += b;
    ````
 + 인터페이스 앞에는 I를 붙일 것.

 + **<주석>**
   - /* */문을 사용한다.
    ````c
    ex)
    /*
    <summary>
    함수에 대한 설명
    <params>
    인수에 대한 설명
    <return>
    반환값에 대한 설명
    */
    void asdf(int a, char b) {...
    ````
   - 변수 선언부의 주석은 ; 뒤에 주석을 붙인다. (//+" "+내용)
    ````c
    ex)
    public int temp; // 임시변수
    ````
   - 주석을 작성할 시, 키워드 위에 작성해야 한다.
    ````c    
    ex)
    // 아이템 가져오기
    temp += item2;
    ````
   -
## [약어](진행하면서 추가할 예정)
 + temp? tmp?
