function [epos, IPOS, rsq] = model1(Y, X1, step, start, X0)
%============================================
%�������Ĺ�Ʊծȯ��λ����ҵ����ģ��
%   --����ָ���ع�
%============================================
%���룺
%[�����������������������]
%[��ֵ��5.23%��Ϊ5.23]
%   Y:  ����ֵ������������
%   X1: ��ҵָ��������������
%   step: ���㲽��
%   start: ��ǰ����λ��
%   X0: ��ʼ��
%�����
%   epos: ��Ʊ��λ
%   IPOS: ��ҵ��������
%   rsq : �ع���Ͷ�
%ģ�ͣ�
%   ָ���ع鷨
%============================================
    %��������
    n=step;         %ʱ�����г���
    tposmax=0.95;   %��Ʊ��ծȯ�ϼ���߲�λ
    eposmin=0;      %��Ʊ��Ͳ�λ����
        
    %��Ʊָ��
    X1N = X1(start-n+1:start,:);
    [~,m]=size(X1N); 
            
    %��������ʽԼ��������  AX    <     B
    %   0	1  1 ...  1              tposmax
    %   0  -1 -1 ... -1             -eposmin
    %   0  -1                         0
    %  ...    -1                      0
    %   0            -1               0
    
    A=[ ones(1,m);
       -ones(1,m);
        -eye(m)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        zeros(m,1);
        ];
    
    %�����ع����:
%     XN=[ones(n,1),X1N,X2N];  %��һ��Ϊȫ1����ʾ������
    XN=[ones(n,1),X1N];
    YN=Y(start-n+1:start);

    %��Լ�������Իع飺��С���˷�
    options = optimset('MaxIter', 10000);
    [BETA,ssr] = lsqlin(XN,YN,A,B,[],[],[],[], X0,options);    
    
    %����R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %������R^2
    
    %��ҵ����ϵ��
    IPOS = BETA(2:m+1);

    %�����λ
    epos = sum(IPOS);
end
