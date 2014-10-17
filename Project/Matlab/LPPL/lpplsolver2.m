%================================
%Sover1: ����ʱ���������tc�ķ�Χ
%Ҫ�㣺
%   1������t2���ƶ���tc��Ԥ��Ҳ���ƶ������t2�ı䶯��Ӧ��̫��
%   2�����������ʱ�����ò����߽�����޷�������ƥ�䣬���鲻��߽���ڽ����ɸѡ
%   3) ���ս����Ӧ���Բв��С�ľ�������Ӧ�ÿ���tc�ķֲ�
%================================
clear;

%��������
% data = csvread('D:\Projects\Matlab\LPPL\data\index1.csv',1,2);  %������
% pxcol=2;    %1=��֤��ָ 2=��С��ָ 3=��ҵ����
% gstart=2325;%����ʱ�����
% gend=2800;  %����ʱ���յ�
%ʱ������:  ������
%      1 = 2000-01-05
%   1297 = 2005-06-06   ��֤��ָ�� 998��
%   1997 = 2007-10-16   ��֤��ָ��6124��
%   2125 = 2008-10-29   ��С��ָ��2114��
%   2480 = 2010-04-13   ��С��ָ��6177��
%   2620 = 2010-11-11   ��С��ָ��7493��
%   3124 = 2012-12-04   ��ҵ���ۣ� 575��
%   3301 = 2013-08-30   
%ָ��˳��
%   ��֤��ָ/��С��ָ/��ҵ����

data = csvread('D:\Projects\Matlab\LPPL\data\index2.csv',1,2);
pxcol=2;    %1=��֤��ָ 2=��С��ָ 3=��ҵ����
gstart=2325;%����ʱ�����
gend=2800;  %����ʱ���յ�
%ʱ������:  ������
%      1 = 2000-01-02
%   1983 = 2005-06-06   ��֤��ָ�� 998��
%   2845 = 2007-10-16   ��֤��ָ��6124��
%   3224 = 2008-10-29   ��С��ָ��2114��
%   3755 = 2010-04-13   ��С��ָ��6177��
%   3967 = 2010-11-11   ��С��ָ��7493��
%   4721 = 2012-12-04   ��ҵ���ۣ� 575��
%   4990 = 2013-08-30   
%ָ��˳��
%   ��֤��ָ/��С��ָ/��ҵ����


gstep=5;    %���ò���
gminlen=60; %������С���ݵ�

%���ò���ʱ���
% t1list=gstart:gstep:(gstart+gstep*5);
t1list=gstart;

%�����¼
R1=zeros(length(t1list)*(gend-(gstart+60)+1)/gstep,12);
R2=R1;
rcount=1;

%��ʼ��������
hwait = waitbar(0,'0%');
    
for i=1:length(t1list)
    t1=t1list(i);
    t2list=(t1+gminlen):gstep:gend;
    for j=1:length(t2list)
        t2=t2list(j);
        P=data(t1:t2,pxcol);
        
        %----------------------
        %���LPPLģ��
        %----------------------
        LNP=log(P);
        N =length(LNP);
        T=(1:N)';

        para=[0.5;8;N+40];  %��ʼ����������        
        lb=[];              %���ñ߽��޷�������ƥ�䣬���鲻��߽��ڽ����ɸѡ
        ub=[];
        options = optimset('Algorithm','Levenberg-Marquardt');
        [para,resnorm] = lsqcurvefit(@lpplfitfun,para,[T LNP],LNP,lb,ub,options); 
        [~, P] = lpplfitfun(para, [T LNP]);
        % 1)0.1<=m<=0.9     
        % 2)6<=w<=13  
        % 3)tc>t
        B=P(2); m = P(5); w=P(6); tc=P(7);
        if m>0 & m<1
            flag=1;
        else
            flag=0;
        end;        
        R1(rcount,:) = [resnorm t1 t2 P' fix(t1+tc) flag];
        
        %Ϊ�˱����޿��ý�������ñ߽���ϲ�����Ϊ��ѡ
        lb=[0.01;6;N+1];
        ub=[0.9;13;N+100]; 
        [para,resnorm] = lsqcurvefit(@lpplfitfun,para,[T LNP],LNP,lb,ub); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R2(rcount,:) = [resnorm t1 t2 P' fix(t1+P(7)) 1];
        
        rcount=rcount+1;
        
        %���½�����
        len1=length(t1list);
        len2=length(t2list);
        waitbar(((i-1)*len1+j)/(len1*len2),hwait,[num2str(fix(((i-1)*len1+j)/(len1*len2)*100)),'%']);
    end;
end;

%�رս�����
close(hwait);

%ɸѡ����
% 1)0.1<=m<=0.9     
% 2)6<=w<=13  
% 3)tc>t
R1=R1(R1(:,8)<1 & R1(:,8)>0,:); %0<m<1��Col=8

% R1=R1(R1(:,1)>0,:);
% R2=R2(R2(:,1)>0,:);

%����������ͼ,�໭K��
N=length(data(:,pxcol));
lastpx=data(N,pxcol);
K=100;
t1=gstart;t2=gend;
if N>=(t2 + K)
    PY=data(t1:t2+K,pxcol);
else
    M=t2+K-N;
    PY=[data(t1:N,pxcol);ones(M,1)*lastpx;];
end;

subplot(2,1,1);
PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
%���ߣ��ޱ߽�Լ�����
TC=fix(R1(:,10));
TC=TC(TC<length(PY));
NY=PY(TC);
plot(TC,NY,'rx');
for i=1:length(R1(:,1))
    t2=R1(i,3)-R1(i,2)+1;
    tc=fix(R1(i,10)) ;
    if tc < length(PY)
        yest=PY(tc);
        X1=[t2,t2,tc];
        Y1=[0,yest,yest];
        plot(X1,Y1,'b');
    end;
end;


subplot(2,1,2);
PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
%���ߣ��߽�Լ�����
TC=fix(R2(:,10));
TC=TC(TC<length(PY));
NY=PY(TC);
plot(TC,NY,'bo');
for i=1:length(R2(:,1))
    t2=R2(i,3)-R2(i,2)+1;
    tc=fix(R2(i,10)) ;
    if tc < length(PY)
        yest=PY(tc);
        X1=[t2,t2,tc];
        Y1=[0,yest,yest];
        plot(X1,Y1,'r');
    end;
end;
%LOMBƵ�׷�����[H,q]������Ornstein-Uhlenbeck mean-reverting    