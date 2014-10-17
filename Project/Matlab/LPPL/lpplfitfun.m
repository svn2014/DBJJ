function [F, P] = lpplfitfun(PARA, DATA)
%���ڷ��������LPPLģ�͵ĺ�������lsqcurvefit����

    %��������
    m=PARA(1);
    w=PARA(2);
    tc=PARA(3);
    
    %����
    T=DATA(:,1);
    LNP=DATA(:,2);
    
    %�м����
    N=length(T);
    F=(tc-T).^m;
    G=F.*cos(w.*log(tc-T));
    H=F.*sin(w.*log(tc-T));    
    sumf=sum(F);
    sumg=sum(G);
    sumh=sum(H);
    sumff=sum(F.*F);
    sumfg=sum(F.*G);
    sumfh=sum(F.*H);
    sumgg=sum(G.*G);
    sumgh=sum(G.*H);
    sumhh=sum(H.*H);
    sumy=sum(LNP);
    sumyf=sum(LNP.*F);
    sumyg=sum(LNP.*G);
    sumyh=sum(LNP.*H);
    
    %������Բ���
    X=[N sumf sumg sumh;sumf sumff sumfg sumfh; sumg sumfg sumgg sumgh;sumh sumfh sumgh sumhh];
    Y=[sumy;sumyf;sumyg;sumyh];
    M=X\Y;  %Warning: Matrix is close to singular or badly scaled. Results may be inaccurate. RCOND =  6.555192e-25. 
    A=M(1);B=M(2);C=M(3);D=M(4); 
    
    %�������
    F = A+B.*((tc-T).^m)+C.*((tc-T).^m).*cos(w.*log(tc-T))+D.*((tc-T).^m).*sin(w.*log(tc-T));
    P = [M;m;w;tc];
end

