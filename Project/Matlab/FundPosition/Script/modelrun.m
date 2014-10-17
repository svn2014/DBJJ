function [EST,IND,ERR,RSQ,REAL] = modelrun(model)
    %����ʾWarning
    warning('off');

    %���ò���
%     model=3;        %1=����ָ���ع�   2=����ָ���ع�    
    switch model
        case 1
            step=30;
            indextype=1;    %1=����һ����ҵ��28��
        case 2
            step=15;
            indextype=5;    %5=��֤100/300/500
        otherwise
            step=28;
            indextype=1;
    end
    indexopt=[];    
    fundtype=0;     %0=�ɺ��Ի������ͣ� 1=��Ʊ��    
    usefundgroup=1; %1=ȫ�������ܺ�
    
    %�Ƿ���ڼ��������ղ�����ͳ�����
    checkerronly=0;
    
    %��ʼ��������
    hwait = waitbar(0,'0%');
    
    %�������ݣ���ʱ����������
    [YN, X1N, X2N, PN, ~] = getdata(usefundgroup,indextype,indexopt);

    %����������
    [~,colind] = size(X1N);
    [rowf, colf] = size(YN);

    %��������λ�����ͳ��
    EST=zeros(rowf, colf);
    IND=zeros(rowf, colind);
    RSQ=zeros(rowf, colf);
    ERR=zeros(rowf, colf);
    REAL=PN;
    X0=[];  %������ʼֵ
    
    n=step;
    for i=n:rowf
        if checkerronly==1 && sum(PN(i,:)) > 0 || checkerronly~=1
            for j=1:colf
                Y=YN(:,j);         
                %������
                [epos, ~, IPOS, rsqr, BETA] = estfundpos(Y, X1N, X2N, n,fundtype, model, i, X0);
                X0=BETA;    %������ʼֵ
                
                EST(i,j)=epos;
                IND(i,:)=IPOS';
                RSQ(i,j)=rsqr;
            end

            %���ͳ��
            if sum(PN(i,:)) > 0
                ERR(i,:) = EST(i,:) - PN(i,:);
            end
        end

        %���½�����
        waitbar((i-n+1)/(rowf-n+1),hwait,['M',num2str(model),'-I',num2str(indextype),'-S',num2str(n),':  ',num2str(floor((i-n+1)/(rowf-n+1)*100)),'%']);
    end

    %�ܽ����
    errhist=ERR(sum(ERR,2)~=0,:);
    
    %�رս�����
    close(hwait);
    
    %��ͼ
    subplot(2,2,1);
    plot(EST(:,1));hold on;plot(REAL(:,1));    
    hold off;
    title('��λԤ��');
    subplot(2,2,2);
    plot(RSQ(:,1));
    title('����Ŷ�');
    subplot(2,2,3);
    plot(ERR(:,1));
    title('Ԥ�����');
    subplot(2,2,4);
    hist(errhist);
    title('���ֲ�');
    set(gcf,'outerposition',get(0,'screensize'));    
end