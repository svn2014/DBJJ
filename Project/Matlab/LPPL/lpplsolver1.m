%================================
%Sover1: ����ʱ���������tc�ķ�Χ
%Ҫ�㣺
%   1������t2���ƶ���tc��Ԥ��Ҳ���ƶ������t2�ı䶯��Ӧ��̫��
%   2�����������ʱ�����ò����߽�����޷�������ƥ�䣬���鲻��߽���ڽ����ɸѡ
%   3) ���ս����Ӧ���Բв��С�ľ�������Ӧ�ÿ���tc�ķֲ�
%================================
clear;clc;

%��������
data = csvread('D:\Projects\Matlab\LPPL\data\index1.csv',1,2);  %������
pxcol=1;    %1=��֤��ָ 2=��С��ָ 3=��ҵ����
% t1=1525;t2=1854;
t1=1450;t2=1834;
%ʱ������:  ������
%      1 = 2000-01-05
%   1297 = 2005-06-06   ��֤��ָ�� 998��
%   1997 = 2007-10-16   ��֤��ָ��6124��
%       2125 = 2008-10-29   ��С��ָ��2114��
%       2480 = 2010-04-13   ��С��ָ��6177��
%           2620 = 2010-11-11   ��С��ָ��7493��
%           3124 = 2012-12-04   ��ҵ���ۣ� 575��
%           3301 = 2013-08-30   
%ָ��˳��
%   ��֤��ָ/��С��ָ/��ҵ����

% data = csvread('D:\Projects\Matlab\LPPL\data\index2.csv',1,2);
% pxcol=1;    %1=��֤��ָ 2=��С��ָ 3=��ҵ����
% t1=2100;t2=2800;
%ʱ������:  ������
%      1 = 2000-01-02
%   1983 = 2005-06-06   ��֤��ָ�� 998��
%   2845 = 2007-10-16   ��֤��ָ��6124��
%       3224 = 2008-10-29   ��С��ָ��2114��
%       3755 = 2010-04-13   ��С��ָ��6177��
%           3967 = 2010-11-11   ��С��ָ��7493��
%           4721 = 2012-12-04   ��ҵ���ۣ� 575��
%           4990 = 2013-08-30   
%ָ��˳��
%   ��֤��ָ/��С��ָ/��ҵ����
    
%����֪ʱ�������и�ȡ15����ͬ��t1��t2��ϸ�����
%ÿ��t1,t2���5����Ͻ������R
%����t2���ƶ���tc��Ԥ��Ҳ���ƶ�
tstep1=2;tstep2=1;
tstepcnt1=5;tstepcnt2=5;
% t1list = t1:tstep1:(t1+tstep1*tstepcnt1);
% t2list = (t2-tstep2*tstepcnt2):tstep2:t2;
t1list=t1;
t2list=t2;

%���ԣ����䶯
% t1list=t1;t2list=t2;
R1=zeros(length(t1list)*length(t2list),12);
R2=R1;

rcount=1;
for j=1:length(t1list)
    for k=1:length(t2list)
        P=data(t1list(j):t2list(k),pxcol);
        
        %----------------------
        %���LPPLģ��
        %----------------------
        LNP=log(P);
        N =length(LNP);
        T=(1:N)';

        %��ʼ����߽�����
        %   ���ܵľ��棺����ӽ����죬��det(Matrix)�ӽ�0���ⷽ����õ��Ľ�����ܲ���ȷ��RCOND���������ĵ�����RCONDԽС����Խ�ӽ����죡
        %   ���飺�������һ����Ȼ����������ֵ�ٵ�����ֵ�����㣬ֱ����������Ϊֹ
        x0=[0.5;8;N+80];
        % 1)0.1<=m<=0.9     
        % 2)6<=w<=13  
        % 3)tc>t
        %���ñ߽��޷�������ƥ�䣬���鲻��߽��ڽ����ɸѡ
        lb=[];
        ub=[];
        %��������ϲ���������
        options = optimset('Algorithm','Levenberg-Marquardt');
        %[t1list(j) t2list(k)]        
        [para,resnorm, ~, exitflag] = lsqcurvefit(@lpplfitfun,x0,[T LNP],LNP,lb,ub,options); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R1(rcount,:) = [resnorm t1list(j) t2list(k) P' fix(t1list(j)+P(7)) exitflag];
        
        %Ϊ�˱����޿��ý�������ñ߽���ϲ�����Ϊ��ѡ
        lb=[0.0001;6;N+1];
        ub=[0.9999;13;N+100]; 
        [para,resnorm, ~, exitflag] = lsqcurvefit(@lpplfitfun,x0,[T LNP],LNP,lb,ub); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R2(rcount,:) = [resnorm t1list(j) t2list(k) P' fix(t1list(j)+P(7)) exitflag];

        rcount=rcount+1;
    end    
end

%ɸѡ����
% 1)0.1<=m<=0.9     
% 2)6<=w<=13  
% 3)tc>t
R1=R1(R1(:,8)<1 & R1(:,8)>0,:); %0<m<1��Col=8
R1=R1(R1(:,10)>0,:);            %tc>0��Col=10

%����������ͼ,�໭K��
N=length(data(:,pxcol));
lastpx=data(N,pxcol);
K=100;
if N>=(t2 + K)
    PY=data(t1:t2+K,pxcol);
else
    M=t2+K-N;
    PY=[data(t1:N,pxcol);ones(M,1)*lastpx;];
end;

PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
plot([t2-t1+1,t2-t1+1],[0,PY(t2-t1+1)],'k');


%���ߣ��ޱ߽�Լ����� + ɸѡ
[N,~]=size(R1);
for i=1:N
    A=R1(i,4);B=R1(i,5);C=R1(i,6);D=R1(i,7); m=R1(i,8);w=R1(i,9);tc=R1(i,10);
    
    %�����м���abs����tc֮��ĵ�
    NY=exp(A+B.*(abs(tc-PT)).^m+C.*(abs(tc-PT)).^m.*cos(w.*log(abs(tc-PT)))+D.*(abs(tc-PT)).^m.*sin(w.*log(abs(tc-PT))));
    plot(PT,NY,'r');
end;

%���ߣ��߽�Լ�����
[N,~]=size(R2);
for i=1:N
    A=R2(i,4);B=R2(i,5);C=R2(i,6);D=R2(i,7); m=R2(i,8);w=R2(i,9);tc=R2(i,10);
    
    %�����м���abs����tc֮��ĵ�
    NY=exp(A+B.*(abs(tc-PT)).^m+C.*(abs(tc-PT)).^m.*cos(w.*log(abs(tc-PT)))+D.*(abs(tc-PT)).^m.*sin(w.*log(abs(tc-PT))));
    plot(PT,NY,'b');
end;
    
    
    
    




    %����ʹ��LM�㷨
    %LOMBƵ�׷�����[H,q]������Ornstein-Uhlenbeck mean-reverting