function [ epos, bpos, IPOS, rsq, BETA] = estfundposmodel1(Y, X1, X2, step, type, start, X0)
%============================================
%�������Ĺ�Ʊծȯ��λ����ҵ����ģ��
%   --����ָ���ع�
%============================================
%���룺
%[�����������������������]
%[��ֵ��5.23%��Ϊ5.23]
%   Y:  ����ֵ������������
%   X1: ��ҵָ��������������
%   X2: ծȯָ��������������
%   step: ʱ�����г���
%   type: �������ͣ�1=��Ʊ�ͣ�2=�����
%   start: ��ǰ����λ��
%�����
%   epos: ��Ʊ��λ
%   bpos: ծȯ��λ
%   IPOS: ��ҵ��������
%   rsq : �ع���Ͷ�
%ģ�ͣ�
%   ָ���ع鷨
%============================================
    %��������
    n=step;         %ʱ�����г���
    tposmax=0.95;   %��Ʊ��ծȯ�ϼ���߲�λ
    switch type
        case 1  %��Ʊ��
            eposmin=0.6;	%��Ʊ��Ͳ�λ����
            bposmax=0.4;	%ծȯ��߲�λ����
        otherwise
            eposmin=0;
            bposmax=1;
    end
        
    %��Ʊָ��
    X1N = X1(start-n+1:start,:);
    [~,col1]=size(X1N);   
    
    %ծȯָ��
    X2N = X2(start-n+1:start,1);
    [~,col2]=size(X2N);

    %�Ա�������
    m=col1+col2;
            
    %��������ʽԼ��������  AX    <     B
    %   0	1  1 ...  1   1           tposmax
    %   0  -1 -1 ... -1   0          -eposmin
    %   0   0  0 ...  0   1           bposmax
    %   0  -1                         0
    %  ...    -1                      0
    %   0                -1           0
    
    A=[ ones(1,m);
       -ones(1,col1),zeros(1,col2);
        zeros(1,col1),ones(1,col2);
        -eye(m)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        bposmax;
        zeros(m,1);
        ];
    
    %�����ع����:
    XN=[ones(n,1),X1N,X2N];  %��һ��Ϊȫ1����ʾ������
    YN=Y(start-n+1:start);

    %��Լ�������Իع飺��С���˷�
    options = optimset('MaxIter', 10000);
    [BETA,ssr] = lsqlin(XN,YN,A,B,[],[],[],[], X0,options);    
    
    %����R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %������R^2
    
    %��ҵ����ϵ��
    IPOS = BETA(2:col1+1);

    %�����λ
    epos = sum(IPOS);
    bpos = sum(BETA((1+col1+1):(1+m))); 
end

