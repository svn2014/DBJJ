function [ Y, X1, X2, P, S] = getdata( isgroup, indextype, N )
%��ȡ����
%   Y: ����ֵ������
%   X1:��Ʊָ��������
%   X2:ծȯָ��������
%   P: ��ʵ������λ
%   S: ���������־
%
%   isgroup:    1=�����л�����Ϊ���� 0=��������
%   indextype:  ��Ϊ������ָ������
%   N: ����������������

     %��������
%     idata=csvread('..\data\index.csv',1,1);         %��ҵ��ģծȯָ��
%     fdata=csvread('..\data\fundnav.csv',1,1);       %����ֵ������
%     sdata=csvread('..\data\fundsetup.csv',1,1);     %���������־
%     pdata=csvread('..\data\fundposition.csv',1,1);  %����������λ
%     fpxdata=csvread('..\data\fundpx.csv',1,1);      %����ֵ
%     fshdata=csvread('..\data\fundshare.csv',1,1);   %����ݶ�Ѿ���21���ӳٴ���
    
    %��������
    idata=csvread('..\data\index2.csv',1,1);         %��ҵ��ģծȯָ��
    fdata=csvread('..\data\fundnav2.csv',1,1);       %����ֵ������
    pdata=csvread('..\data\fundposition2.csv',1,1);  %����������λ
    fpxdata=csvread('..\data\fundpx2.csv',1,1);      %����ֵ
    fshdata=csvread('..\data\fundshare2.csv',1,1);   %����ݶ�Ѿ���21���ӳٴ���
    
    %===========================
    %����
    %   ��ʱ���������У����������г���һ�£�������С240�У�1�꣩
    %   ������ʱ�䣺2005/1/1-2011/12/31
    %   ������ʱ�䣺2012/1/1-2013/01/24
    %   indextype = 1: ����һ����ҵָ�� [1:23]
    %               2: Windһ����ҵָ�� [24:33]
    %               3: Wind֤�����ҵָ�� [34:56]
    %               4: ����һ����ҵָ�� [57:85]
    %               5: ��֤100/200/500��ģָ�� [86:88]
    %               6: �����/��/С��ģָ�� [89:91]
    %               7: �޳���/��/С��ģָ�� [94,96]
    %               8: �޳���ģ+���ָ�� [97,102]
    %               9: �޳�1000��ҵָ�� [103,112]
    %               10:������ӯ��ָ�� [113,115]
    %               11:�����о���ָ�� [116,118]
    %               12:����ɼ�ָ�� [119,121]
    %               13:����ӯ��ָ�� [122,124]
    %               14:��̩��ҵָ�� [125,147]
    %               15:���ŷ��ָ�� [148,152]
    %   ծȯָ��: ��֤ȫծ [92]
    %   תծָ��: �б��תծ[93]    
    indextypelist=[1,23
     24,33
     34,56
     57,85
     86,88
     89,91
     94,96
     97,102
     103,112
     113,115
     116,118
     119,121
     122,124
     125,147
     148,152];
 
    if indextype==0
        X1=idata(:,N)/100;
    else
        X1=idata(:,indextypelist(indextype,1):indextypelist(indextype,2))/100;
    end
    
    X2=idata(:,92)/100;
    
    if isgroup==1
        %===========================
        %�ϲ����л������
        %===========================
        %����������ʲ�
        fasset=fpxdata.*fshdata;        
        totalshare=sum(fshdata,2);
        totalasset=sum(fasset,2);

        %��������𹫲���λ
        feqp=sum(fpxdata.*fshdata.*pdata,2)./sum(fpxdata.*fshdata,2);

        %����ȫ�����𼯺ϵ�������
        fappr=zeros(size(fasset),1);
        for fa=2:size(fasset)
            share1=totalshare(fa,1);
            share0=totalshare(fa-1,1);
            if share1==share0
                %����֮ǰ����ݶ��
                fappr(fa)=totalasset(fa)/totalasset(fa-1)-1;
            else
                %����֮��ݶ����
                fappr(fa)=(fpxdata(fa,:)*fshdata(fa,:)')/(fpxdata(fa-1,:)*fshdata(fa,:)')-1;
            end
        end
        
        Y=fappr(2:end);        
        P=feqp(2:end)/100;
        X1=X1(2:end,:);
        X2=X2(2:end,:);
        S=ones(size(Y));
    else
        %===========================
        %���㵥������
        %===========================
        Y=fdata(:,N)/100;
        P=pdata(:,N)/100;
        S=sdata(:,N);
    end
    
    
end

