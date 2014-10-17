function [ epos, bpos, IPOS, rsq] = estfundposmodel3(Y, X1, X2, step, type, start)
%============================================
%�������Ĺ�Ʊծȯ��λ����ҵ����ģ��
%   --���ɷݻع鷨
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
%   ָ�����ɷݻع鷨
%============================================
    %��������
    n=step;            %ʱ�����г���
    tposmax=0.95;      %��Ʊ��ծȯ�ϼ���߲�λ
    reqvecnum=30;       %Ҫ��������������
    reqexplained=90;   %Ҫ���ۼƽ��Ͷ�, ��reqvecnum=0ʱ��Ч
        
    switch type
        case 1  %��Ʊ��
            eposmin=0.6;	%��Ʊ��Ͳ�λ����
            bposmax=0.4;	%ծȯ��߲�λ����
        otherwise
            eposmin=0;
            bposmax=1;
    end
            
    %���ɷݷ���
    %   coeff: ��������
    %   score: ���ɷݾ���
    %   latent: ����ֵ
    %   explained: ���Ͷ�
    XT=X1(start-n+1:start,:);
    XSTD=std(XT);
    [COEFF, SCORE, ~, ~, EXPLAINED] = pca(zscore(XT));
    
    if reqvecnum==0
        cont=0;
        num=0;
        while (cont<reqexplained && num<length(EXPLAINED))
            num=num+1;
            cont=cont+EXPLAINED(num);
        end
    else
        num = min(reqvecnum,length(EXPLAINED));
    end
    
    %��������
    VEC=COEFF(:,1:num);
    [row3,~]=size(VEC);
    XSTD=XSTD(1:num);  %��׼��
    
    %��Ʊָ��: ͨ�����ɷݹ���
    XN1 = SCORE(1:n,1:num);
    [~,col1]=size(XN1);
    
    %ծȯָ��
    XN2 = (X2(start-n+1:start,1));
    [~,col2]=size(XN2);

    %�Ա�������
    m=col1+col2;
    
    %��������ʽԼ��������  AX    <     B
    %   0	sum(ai1)    sum(ai2)    1           tposmax
    %   0  -sum(ai1)   -sum(ai2)    0          -eposmin
    %   0   0           0           1           bposmax
    %   0   0           0          -1           0
    %   0  -a11        -a12         0           0
    %  ...                          0           0
    %   0  -an1        -an2         0           0
    
    A=[ sum(VEC),ones(1,col2);
       -sum(VEC),zeros(1,col2);
        zeros(1,col1),ones(1,col2);
        zeros(1,col1),-ones(1,col2);
        -VEC,zeros(size(VEC),col2)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        bposmax;
        0;
        zeros(size(VEC),1);
        ];
    
    %�����ع����:
    XN=[ones(n,1),XN1,XN2];  %��һ��Ϊȫ1����ʾ������
    YN=(Y(start-n+1:start));

    %��Լ�������Իع飺��С���˷�
    [BETA,ssr] = lsqlin(XN,YN,A,B);
    
    %����R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %������R^2
    
    %��ҵ����ϵ��
    IPOS = (VEC./repmat(XSTD,row3,1))*BETA(2:col1+1);

    %�����λ
    epos = sum(IPOS);
    bpos = BETA((1+col1+1):(1+m)); 
end

