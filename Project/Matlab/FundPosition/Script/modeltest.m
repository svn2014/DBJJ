function [ERRSUM,ERRLIST,RSQLIST] = modeltest(model)
    %����ʾWarning
    warning('off');

    %���ò���
    fundtype=0;     %0=�ɺ��Ի������ͣ� 1=��Ʊ��
    stepmin=10;
    stepmax=50;
    
    %�Ƿ���ڼ��������ղ�����ͳ�����
    checkerronly=1;
    
    %��ʼ��������
    hwait = waitbar(0,'0%');    
    ERRSUM=zeros((stepmax-stepmin+1)*6,8);
    ERRLIST=zeros((stepmax-stepmin+1)*6,34);
    RSQLIST=zeros((stepmax-stepmin+1)*6,34);
    
    for indextype=1:15
        %�������ݣ���ʱ����������
        [YN, X1N, X2N, PN, ~] = getdata(1,indextype,4);

        %����������
        [~, coli] = size(X1N);
        [rowf, colf] = size(YN);
        
        %����ָ���ع�
        if model==2
            step0=stepmin;
        else
            step0=max(stepmin,coli+3);    
        end
        

        %��������λ�����ͳ��
        estpos=zeros(rowf, colf);
        estrsqr=zeros(rowf, colf);
        esterr=zeros(rowf, colf);
        X0=[];
        
        for n=step0:stepmax
            for i=n:rowf
                if checkerronly==1 && sum(PN(i,:)) > 0
                    for j=1:colf
                        Y=YN(:,j);         
                        %������
                        [epos, ~, ~, rsqr, BETA] = estfundpos(Y, X1N, X2N, n,fundtype, model, i, X0);
                        X0=BETA;
                        
                        estpos(i,j)=epos;
                        estrsqr(i,j)=rsqr;
                    end

                    %���ͳ��
                    if sum(PN(i,:)) > 0
                        esterr(i,:) = estpos(i,:) - PN(i,:);
                    end
                end
                
                %���½�����
                waitbar((i-n+1)/(rowf-n+1),hwait,['M',num2str(model),'-I',num2str(indextype),'-S',num2str(n),':  ',num2str(floor((i-n+1)/(rowf-n+1)*100)),'%']);
            end

            %�ܽ����
            errhist=esterr(sum(esterr,2)~=0,:);
            rsqrhist=estrsqr(sum(esterr,2)~=0,:);

            %����ͳ����
            ERRLIST((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:)=[indextype, n, errhist'];
            RSQLIST((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:)=[indextype, n, rsqrhist'];
            ERRSUM((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:) = [indextype, n, mean(errhist), mean(abs(errhist)), std(errhist), max(abs(errhist)), length(errhist(abs(errhist)<0.05))/length(errhist),mean(rsqrhist)];    
        end
    end
    
    %�رս�����
    close(hwait);
end