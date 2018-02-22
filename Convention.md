# Project Convention

## [���ϸ�]
+ ���� ���� ��� �빮�ڷ� �����ؾ��ϸ�, Pascal Cae�� �ۼ��ؾ� �Ѵ�.
+ _(�����)�� ������� �� ��.

## [Asset]
+ Texture �� Merarial �� �𵨸� ��� �����ϰ� �� ��.
+ UV mapping �����ʹ� �̸�_UV��� �� ��.

## [�ڵ�����]
+ �Լ�, Ŭ����, �̺�Ʈ ���� PascalCase�� �ۼ� (�빮�ڷ� �����ؼ� �ܾ� ���� ��, �빮�ڷ� ����. ex) PascalCase)
+ �迭 ������ tempArr

+ **<����>**
  - camelCase�� �ۼ�.(�ҹ��ڷ� �����ؼ� �ܾ� �����, �빮�ڷ� ����. ex) camelCase)
  - private������ _�� �����ؼ� ������ ��.
  - �������� �� ������������ �� �ۼ��ؾ��� ��.
  - ���� ���������� ���ο��� �����ִ� �ͳ��� �׷�ȭ��ų ��.
    ````c
    ex)
    public int item1;
    public int item2;
    public int random1;
    public int random2;
    ````
  - �ߺ��� �̸��� ������ ����� ��, 1���� �ѹ��� �� ��.
    ````c
    ex) item1, item2;
    ````
  - ������ ũ�� ������� ������ ��.
  - static -> const -> �Ϲ� ������ ������ ��.
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
+ **<�Լ�>**
  - PascalCase�� ����� ��
  - bool ��ȯ �Լ��� is�� ������ ��. ex) bool IsAbcd();
  - �� �ٿ� �� ������ �ۼ��ؾ��Ѵ�.
  - �ݺ������� ���๮�� �� ���̿��� block�� �ۼ��ؾ��Ѵ�.
  - �Լ� ���ο��� ������ �������� ó���� ������ ��.
  - ����ó���� ���� ���� ������ �� ��!
  - ����� ������ �������� �׷�ȭ�ؼ� �������.
    ````c
    ex)
    a = b;
    c = a + 2;

    for(......

    a = c;
    c += b;

    for(......
    ````
   - ��ü ����
    ````c
    bool IsAsdf(int a, int b, bool isValid){
       // �����
       int c;
       int item;

       // ���Ժ�
       if(!isValid){
          return false;
       }

       // �Ҵ��(�Ҵ��� �߰��� �� �� �� �ִ�.)
       c = a;
       item = b;

       // �Լ� ����
       for(..........
       ...
    }
    ````
 + **<Ŭ����>**
   - Ŭ���� �̸��� �ݵ�� �빮�ڷ� �����ؾ��Ѵ�.
   - Ŭ���� �� ���� �� �Լ����� ���� �����ڸ� �ݵ�� �ۼ��ؾ��Ѵ�. ex) public, protected, private
   - ���� ������ ������ public, protected, private �̴�.
   - ��� ������ �տ� m�� ���� ��.
   - getter, setter �Լ��� class���� �������� �ۼ��� ��.
   - ��� �Լ��� ���� �� ������� �ۼ��� ��.
   - ��� ������ �����ڳ��� �ۼ��صΰ� �ٸ� �����ڴ� newline���� ������ ��
    ````c
    ex)
    public int a;
    public int b;

    private int c;
    private int d;

    protected int e;
    protected int f;
    ````


 + �Լ��� ���ڸ� �ۼ��� ��, �� ĭ �������Ѵ�.
     ````c
    ex) transform.Translate(1f, 2f, 3f);
    ````
 + �帧 ���(������ ��)�� ���� ����� �Ѵ�.
     ````c
    ex) while(x == y)
        a += b;
    ````
 + �������̽� �տ��� I�� ���� ��.

 + **<�ּ�>**
   - /* */���� ����Ѵ�.
    ````c
    ex)
    /*
    <summary>
    �Լ��� ���� ����
    <params>
    �μ��� ���� ����
    <return>
    ��ȯ���� ���� ����
    */
    void asdf(int a, char b) {...
    ````
   - ���� ������� �ּ��� ; �ڿ� �ּ��� ���δ�. (//+" "+����)
    ````c
    ex)
    public int temp; // �ӽú���
    ````
   - �ּ��� �ۼ��� ��, Ű���� ���� �ۼ��ؾ� �Ѵ�.
    ````c    
    ex)
    // ������ ��������
    temp += item2;
    ````
   -
## [���](�����ϸ鼭 �߰��� ����)
 + temp? tmp?
