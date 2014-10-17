function [ epos, IPOS, rsq] = model2(Y, X1, step, start, X0)
%============================================
%�������Ĺ�Ʊծȯ��λ����ҵ����ģ��
%   --����ָ���ع鷨
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
%   IPOS: ��ģ��������
%   rsq : �ع���Ͷ�
%ģ�ͣ�
%   �ϳ�ָ���ع鷨
%============================================
    %��������
    n=step;         %ʱ�����г���

    %������
    XN=X1(start-n+1:start,:);
    YN=Y(start-n+1:start);
    [~,m]=size(XN);
    
    %���������Ż�: 
    H=2*(XN')*XN;
    F=-2*(YN')*XN;    
    A=-eye(m);
    B=zeros(m,1);
    Aeq=ones(1,m);
    Beq=1;
    W=quadprog(H,F,A,B,Aeq,Beq);
    
    %���ݶ����Ż������ع����:
    X=[ones(n,1),XN*W];  %��һ��Ϊȫ1����ʾ������
    [B,~,~,~,STATS] = regress(YN,X);
    
    epos = B(2);
    IPOS=W*epos;
    rsq=STATS(1);
end
